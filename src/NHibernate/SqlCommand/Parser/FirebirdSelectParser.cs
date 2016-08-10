using System;
using System.Collections.Generic;
using static NHibernate.SqlCommand.Parser.MsSqlSelectParser;

namespace NHibernate.SqlCommand.Parser
{
	public class FirebirdSelectParser
	{
		private readonly SqlString _sql;
		private int _paramPos;
		private readonly IList<ColumnDefinition> _columns = new List<ColumnDefinition>();

		public FirebirdSelectParser(SqlString sql)
		{
			_sql = sql;
			ParamsToTypeCast = new List<FbParamDef>();
		}

		public void Parse()
		{
			var tokenizer = new SqlTokenizer(_sql).GetEnumerator();
			tokenizer.MoveNext();

			SqlToken selectToken;
			bool isDistinct;
			if (tokenizer.TryParseUntilFirstMsSqlSelectColumn(out selectToken, out isDistinct))
			{
				ParseSelectClause(tokenizer);

				if (TryParseUntil(tokenizer, "where"))
					ParseWhereClause(tokenizer);

				tokenizer = new SqlTokenizer(_sql).GetEnumerator();
				tokenizer.MoveNext();

				if (TryParseUntil(tokenizer, "order"))
					ParseOrderByClause(tokenizer);
			}
		}

		public IList<FbParamDef> ParamsToTypeCast { get; private set; }

		private void ParseSelectClause(IEnumerator<SqlToken> tokenizer)
		{
			SqlToken prevToken = null;
			var caseBlock = 0;

			do
			{
				var currentToken = tokenizer.Current;
				if (currentToken == null)
					break;

				switch (currentToken.TokenType)
				{
					case SqlTokenType.Text:
						if (currentToken.Equals("from", StringComparison.InvariantCultureIgnoreCase))
						{
							return;
						}

						if (currentToken.Equals("case", StringComparison.InvariantCultureIgnoreCase))
						{
							caseBlock++;
							break;
						}

						if (currentToken.Equals("end", StringComparison.InvariantCultureIgnoreCase))
						{
							if (caseBlock > 0)
							{
								caseBlock--;
								break;
							}
						}

						break;

					case SqlTokenType.Parameter:
						_paramPos++;

						if (prevToken.Equals("first", StringComparison.InvariantCultureIgnoreCase))
							break;

						if (prevToken.Equals("skip", StringComparison.InvariantCultureIgnoreCase))
							break;

						if (caseBlock > 0)
						{
							if (prevToken.Equals("then", StringComparison.InvariantCultureIgnoreCase))
							{
								ParamsToTypeCast.Add(new FbParamDef(_paramPos - 1));
							}
							break;
						}

						ParamsToTypeCast.Add(new FbParamDef(_paramPos - 1));
						break;
				}

				prevToken = currentToken;

			}
			while (tokenizer.MoveNext());
		}

		private static bool TryParseUntil(IEnumerator<SqlToken> tokenizer, string keyword)
		{
			do
			{
				var token = tokenizer.Current;
				if (token != null)
				{
					switch (token.TokenType)
					{
						case SqlTokenType.Text:
							if (token.Equals(keyword, StringComparison.InvariantCultureIgnoreCase))
								return true;
							break;
					}
				}
			}
			while (tokenizer.MoveNext());

			return false;
		}

		private void ParseWhereClause(IEnumerator<SqlToken> tokenizer)
		{
			SqlToken prevToken = null;

			do
			{
				var currentToken = tokenizer.Current;
				if (currentToken == null)
					break;

				switch (currentToken.TokenType)
				{
					case SqlTokenType.Text:
						if (currentToken.Equals("in", StringComparison.InvariantCultureIgnoreCase))
							if (prevToken.TokenType == SqlTokenType.Parameter)
								ParamsToTypeCast.Add(new FbParamDef(_paramPos - 1));
						break;

					case SqlTokenType.Parameter:
						_paramPos++;
						break;
				}

				prevToken = currentToken;

			} while (tokenizer.MoveNext());
		}

		private void ParseOrderByClause(IEnumerator<SqlToken> tokenizer)
		{
			SqlToken prevToken = null;

			do
			{
				var currentToken = tokenizer.Current;
				if (currentToken == null)
					break;

				switch (currentToken.TokenType)
				{
					case SqlTokenType.Text:
						break;

					case SqlTokenType.Parameter:
						_paramPos++;
						if (prevToken.Equals("then", StringComparison.InvariantCultureIgnoreCase))
						{
							ParamsToTypeCast.Add(new FbParamDef(_paramPos - 1));
							break;
						}
						break;
				}

				prevToken = currentToken;

			} while (tokenizer.MoveNext());
		}

		private ColumnDefinition ParseSelectColumnDefinition(SqlToken beginToken, SqlToken endToken, SqlToken aliasToken)
		{
			var name = beginToken == endToken
				? beginToken.Value
				: null;

			var alias = aliasToken.Value;
			var dotIndex = alias.LastIndexOf('.');
			alias = dotIndex >= 0
				? alias.Substring(dotIndex + 1)
				: alias;

			var sqlIndex = beginToken.SqlIndex;
			var sqlLength = (endToken != null ? endToken.SqlIndex + endToken.Length : _sql.Length) - beginToken.SqlIndex;

			return new ColumnDefinition(sqlIndex, sqlLength, name, alias, true);
		}
	}

	public class FbParamDef
	{
		public FbParamDef(int paramPos)
		{
			ParamPos = paramPos;
		}

		public int ParamPos { get; private set; }
	}
}
