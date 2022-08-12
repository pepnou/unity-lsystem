grammar lsystem;
options {
    language=CSharp;
}
FLOAT
	: [0-9]+ ( '.' [0-9]+ )?
	;
VARIABLE
	: [a-z_][a-z0-9_]*
	;
RULENAME
	: [A-Z][a-z0-9_]*
	;
lSystem
	: (assignement ';')* module+ ';' productionRule+ EOF
	;
assignement
	: VARIABLE '=' simpleExpression
	;
productionRule
	: RULENAME ( '(' VARIABLE ( ',' VARIABLE )* ')' )? ('?' probability_ = expression)? (':' predecessors_ += module*)? (':' condition_ = expression)? ':' successors_ += module* ';'
	;
module
	: RULENAME '(' expression ( ',' expression )*  ')'
	| RULENAME
	| moduleLoop
	;
moduleLoop
	: '[' module+ ',' expression ']'
	;
expression
	: var_ = VARIABLE
	| float_ = FLOAT
	| range_ = range
	| '(' expr_ = expression ')'
	| <assoc=right> unop_ = ( '!' | '~' | '-' ) right_ = expression
	| <assoc=left> left_ = expression binop_ = ( '*' | '/' | '%' | '**' ) right_ = expression
	| <assoc=left> left_ = expression binop_ = ( '+' | '-' ) right_ = expression
	| <assoc=left> left_ = expression binop_ = ( '<<' | '>>' ) right_ = expression
	| <assoc=left> left_ = expression binop_ = ( '<=' | '>=' | '<' | '>' ) right_ = expression
	| <assoc=left> left_ = expression binop_ = ( '==' | '!=' ) right_ = expression
	| <assoc=left> left_ = expression binop_ = ( '&' | '^' | '|' ) right_ = expression
	| <assoc=left> left_ = expression binop_ = ( '&&' | '||' ) right_ = expression
	;
range
	: '{' from_ = expression ',' to_ = expression '}'
	;
simpleExpression
	: float_ = FLOAT
	| range_ = range
	| '(' expr_ = simpleExpression ')'
	| <assoc=right> unop_ = ( '!' | '~' | '-' ) right_ = simpleExpression
	| <assoc=left> left_ = simpleExpression binop_ = ( '*' | '/' | '%' | '**' ) right_ = simpleExpression
	| <assoc=left> left_ = simpleExpression binop_ = ( '+' | '-' ) right_ = simpleExpression
	| <assoc=left> left_ = simpleExpression binop_ = ( '<<' | '>>' ) right_ = simpleExpression
	| <assoc=left> left_ = simpleExpression binop_ = ( '<=' | '>=' | '<' | '>' ) right_ = simpleExpression
	| <assoc=left> left_ = simpleExpression binop_ = ( '==' | '!=' ) right_ = simpleExpression
	| <assoc=left> left_ = simpleExpression binop_ = ( '&' | '^' | '|' ) right_ = simpleExpression
	| <assoc=left> left_ = simpleExpression binop_ = ( '&&' | '||' ) right_ = simpleExpression
	;
simpleRange
	: '{' from_ = simpleExpression ',' to_ = simpleExpression '}'
	;
WS
	: [ \t\r\n]+ -> skip
	;