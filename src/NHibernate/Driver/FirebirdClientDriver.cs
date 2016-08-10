using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using NHibernate.Dialect;
using NHibernate.SqlCommand;
using NHibernate.SqlCommand.Parser;
using NHibernate.SqlTypes;
using NHibernate.Util;

namespace NHibernate.Driver
{
	/// <summary>
	/// A NHibernate Driver for using the Firebird data provider located in
	/// <c>FirebirdSql.Data.FirebirdClient</c> assembly.
	/// </summary>
	public class FirebirdClientDriver : ReflectionBasedDriver
	{
		private const string SELECT_CLAUSE_EXP = @"(?<=\bselect|\bwhere).*";
		private const string CAST_PARAMS_EXP = @"(?<![=<>]\s?|first\s?|skip\s?|between\s|between\s@\bp\w+\b\sand\s)@\bp\w+\b(?!\s?[=<>])";
		private readonly Regex _statementRegEx = new Regex(SELECT_CLAUSE_EXP, RegexOptions.IgnoreCase);
		private readonly Regex _castCandidateRegEx = new Regex(CAST_PARAMS_EXP, RegexOptions.IgnoreCase);
		private readonly FirebirdDialect _fbDialect = new FirebirdDialect();

		/// <summary>
		/// Initializes a new instance of the <see cref="FirebirdClientDriver"/> class.
		/// </summary>
		/// <exception cref="HibernateException">
		/// Thrown when the <c>FirebirdSql.Data.Firebird</c> assembly can not be loaded.
		/// </exception>
		public FirebirdClientDriver()
			: base(
				"FirebirdSql.Data.FirebirdClient",
				"FirebirdSql.Data.FirebirdClient",
				"FirebirdSql.Data.FirebirdClient.FbConnection",
				"FirebirdSql.Data.FirebirdClient.FbCommand")
		{
		}

		public override bool UseNamedPrefixInSql
		{
			get { return true; }
		}

		public override bool UseNamedPrefixInParameter
		{
			get { return true; }
		}

		public override string NamedPrefix
		{
			get { return "@"; }
		}

		protected override void InitializeParameter(IDbDataParameter dbParam, string name, SqlType sqlType)
		{
			var convertedSqlType = sqlType;
			if (convertedSqlType.DbType == DbType.Currency)
				convertedSqlType = new SqlType(DbType.Decimal);

			base.InitializeParameter(dbParam, name, convertedSqlType);
		}

		public override IDbCommand GenerateCommand(CommandType type, SqlString sqlString, SqlType[] parameterTypes)
		{
			if(sqlString.IndexOfCaseInsensitive("select") == -1)
				return base.GenerateCommand(type, sqlString, parameterTypes);

			var parser = new FirebirdSelectParser(sqlString);
			parser.Parse();

			if (!parser.ParamsToTypeCast.Any())
				return base.GenerateCommand(type, sqlString, parameterTypes);

			var modifiedSql = new SqlString();
			var paramPos = 0;
			foreach (var part in sqlString)
			{
				var param = part as Parameter;
				if (param == null)
					modifiedSql += new SqlString(part);
				else
				{
					var shouldTypeCast = parser.ParamsToTypeCast.Any(x => x.ParamPos == paramPos);
					if (shouldTypeCast)
					{
						var castType = GetFbTypeFromDbType(parameterTypes[paramPos].DbType);
						modifiedSql += SqlString.Parse(string.Format("cast(? as {0})", castType));
					}
					else
						modifiedSql += new SqlString(part);

					paramPos++;
				}
			}

			return base.GenerateCommand(type, modifiedSql, parameterTypes);
		}

		private string GetStatementsWithCastCandidates(string commandText)
		{
			return _statementRegEx.Match(commandText).Value;
		}

		private HashSet<string> GetCastCandidates(string statement)
		{
			var candidates =
				_castCandidateRegEx
					.Matches(statement)
					.Cast<Match>()
					.Select(match => match.Value);
			return new HashSet<string>(candidates);
		}

		private void TypeCastParam(IDbDataParameter param, IDbCommand command)
		{
			var castType = GetFbTypeFromDbType(param.DbType);
			command.CommandText = command.CommandText.ReplaceWholeWord(param.ParameterName, string.Format("cast({0} as {1})", param.ParameterName, castType));
		}

		private string GetFbTypeFromDbType(DbType dbType)
		{
			return _fbDialect.GetCastTypeName(new SqlType(dbType));
		}
	}
}
