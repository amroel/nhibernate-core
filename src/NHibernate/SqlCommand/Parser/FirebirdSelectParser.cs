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
			int blockLevel = 0;
			SqlToken columnBeginToken = null;
			SqlToken columnEndToken = null;
			SqlToken columnAliasToken = null;

			SqlToken prevToken = null;
			do
			{
				var currentToken = tokenizer.Current;
				if (currentToken == null)
					break;

				columnBeginToken = columnBeginToken ?? currentToken;

				switch (currentToken.TokenType)
				{
					//case SqlTokenType.BracketOpen:
					//	blockLevel++;
					//	break;

					//case SqlTokenType.BracketClose:
					//	blockLevel--;
					//	break;

					case SqlTokenType.Text:
						//if (blockLevel != 0)
						//	break;

						if (currentToken.Equals(",", StringComparison.InvariantCultureIgnoreCase))
						{
							if (columnAliasToken != null)
							{
								_columns.Add(ParseSelectColumnDefinition(columnBeginToken, columnEndToken ?? columnAliasToken, columnAliasToken));
							}
						}

						if (currentToken.Equals("from", StringComparison.InvariantCultureIgnoreCase))
						{
							if (columnAliasToken != null)
							{
								_columns.Add(ParseSelectColumnDefinition(columnBeginToken, columnEndToken ?? columnAliasToken, columnAliasToken));
							}
							return;
						}

						if (currentToken.Equals("as", StringComparison.InvariantCultureIgnoreCase))
						{
							columnEndToken = prevToken;
						}

						if (currentToken.Equals("then", StringComparison.InvariantCultureIgnoreCase))
						{
							if (prevToken.TokenType == SqlTokenType.Parameter)
								ParamsToTypeCast.Add(new FbParamDef(_paramPos - 1));
						}

						columnAliasToken = currentToken;
						break;

					case SqlTokenType.DelimitedText:
						if (blockLevel != 0)
							break;

						columnAliasToken = currentToken;
						break;

					case SqlTokenType.Comma:
						if (blockLevel != 0)
							break;

						if (columnAliasToken != null)
						{
							_columns.Add(ParseSelectColumnDefinition(columnBeginToken, columnEndToken ?? columnAliasToken, columnAliasToken));
						}
						columnBeginToken = columnEndToken = columnAliasToken = null;
						break;

					case SqlTokenType.Parameter:
						_paramPos++;
						if (prevToken.Equals("first", StringComparison.InvariantCultureIgnoreCase))
							break;
						if (prevToken.Equals("skip", StringComparison.InvariantCultureIgnoreCase))
							break;

						ParamsToTypeCast.Add(new FbParamDef(_paramPos - 1));
						break;
				}

				prevToken = currentToken;

			}
			while (tokenizer.MoveNext());

			if (columnAliasToken != null)
			{
				_columns.Add(ParseSelectColumnDefinition(columnBeginToken, columnEndToken ?? columnAliasToken, columnAliasToken));
			}
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
