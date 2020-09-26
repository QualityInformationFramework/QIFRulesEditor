grammar rules;

// dme_rule
// {
//     [if <boolean_expr>]
//     dme_class (must|must not|may) <class>
//         [with xpath <op> <ariphmetic_expr>]
//         ...
//     dme_make_model (must|must not|may)
//         manufacturer = "string"
//         model_number = "string"
//         [serial_number = "string"]
//     dme_id (must|must not|may)
//         <id>
//     ...
// }
// ...

// TODO units?
// TODO desirability
// TODO defaultDesirability
// TODO parameter_value + objectid
// TODO dme_then_id + xid

// Parser Rules

dme_rules: dme_rule* EOF;

dme_rule: DME_RULE_KEYWORD '{' uuid_statement? if_statement? dme_then_statement+ '}';

dme_then_statement
	: dme_then_decision_class
	| dme_then_id
	| dme_then_make_model
	;

dme_then_decision_class:
	DME_THEN_DECISION_CLASS_KEYWORD
	dme_applicability
	ENUM_DME_CLASS
	with_statement*
	;

dme_applicability
	: dme_applicability_must
	| dme_applicability_must_not
	| dme_applicability_may
	;

dme_applicability_must: MUST_KEYWORD;
dme_applicability_must_not: MUST_KEYWORD NOT_KEYWORD;
dme_applicability_may: MAY_KEYWORD;

with_statement: WITH_KEYWORD xpath ((arithmetic_comp_literal arithmetic_expr) | (IN_KEYWORD interval_fn));

// TODO dme_external_id - doc/id
// int | id_asmpath(id, asmpath) | external_id(doc, id) | external_id_asmpath(doc, id, asmpath)
dme_then_id:
	DME_THEN_ID_KEYWORD
	dme_applicability
	INTEGER
	;

dme_then_make_model:
	DME_THEN_MAKE_MODEL_KEYWORD
	dme_applicability
	MANUFACTURER_KEYWORD '=' manufacturer
	MODEL_NUMBER_KEYWORD '=' model_number
	(SERIAL_NUMBER_KEYWORD '=' serial_number)?
	;

manufacturer: STRING_LITERAL;
model_number: STRING_LITERAL;
serial_number: STRING_LITERAL;

uuid_statement : UUID_KEYWORD '=' UUID;

if_statement : IF_KEYWORD boolean_expr;

boolean_expr
	: NOT_KEYWORD boolean_expr                                # boolean_expr_not
	| token_var '=' token_var                                 # boolean_expr_token
    | boolean_expr AND_KEYWORD boolean_expr                   # boolean_expr_and
	| boolean_expr OR_KEYWORD boolean_expr                    # boolean_expr_or
	| arithmetic_expr arithmetic_comp_literal arithmetic_expr # boolean_expr_arithmetic_comparison
	| arithmetic_expr IN_KEYWORD interval_fn                  # boolean_expr_in
    | boolean_literal                                         # boolean_expr_literal
	| boolean_fn                                              # boolean_expr_fn
    | '(' boolean_expr ')'                                    # boolean_expr_braces
    ;

arithmetic_comp_literal
	: arithmetic_comp_literal_equal
	| arithmetic_comp_literal_greater
	| arithmetic_comp_literal_less
	| arithmetic_comp_literal_greater_or_equal
	| arithmetic_comp_literal_less_or_equal
	;

boolean_literal
	: BOOLEAN_TRUE_LITERAL
	| BOOLEAN_FALSE_LITERAL
	;

arithmetic_comp_literal_equal: '=';
arithmetic_comp_literal_greater: '>';
arithmetic_comp_literal_less: '<';
arithmetic_comp_literal_greater_or_equal: '>=';
arithmetic_comp_literal_less_or_equal: '<=';

token_var : token_fn | STRING_LITERAL;

token_fn : TOKEN_KEYWORD '(' xpath ')';

arithmetic_expr
	: '-' arithmetic_expr                                           # arithmetic_expr_negate
	| arithmetic_expr arithmetic_mult_div_literal arithmetic_expr   # arithmetic_expr_mult_div
	| arithmetic_expr arithmetic_plus_minus_literal arithmetic_expr # arithmetic_expr_plus_minus
	| arithmetic_const                                              # arithmetic_expr_const
	| arithmetic_fn                                                 # arithmetic_expr_fn
    | '(' arithmetic_expr ')'                                       # arithmetic_expr_braces
	;

arithmetic_const: INTEGER | DOUBLE;

arithmetic_mult_div_literal
	: arithmetic_mult_literal
	| arithmetic_div_literal
	;

arithmetic_mult_literal: '*';
arithmetic_div_literal: '/';

arithmetic_plus_minus_literal
	: arithmetic_plus_literal
	| arithmetic_minus_literal
	;

arithmetic_plus_literal: '+';
arithmetic_minus_literal: '-';

boolean_fn
	: characteristic_is
	| feature_is_datum
	| feature_is_internal
	| feature_type_is
	| sampling_category_is
	| shape_class_is
	;

characteristic_is: CHARACTERISTIC_IS_KEYWORD '(' ENUM_CHARACTERISTIC ')' ;
feature_is_datum: FEATURE_IS_DATUM_KEYWORD;
feature_is_internal: FEATURE_IS_INTERNAL_KEYWORD;
feature_type_is: FEATURE_TYPE_IS_KEYWORD '(' ENUM_FEATURE ')';
sampling_category_is: SAMPLING_CATEGORY_IS_KEYWORD '(' INTEGER ')';
shape_class_is: SHAPE_CLASS_IS_KEYWORD '(' ENUM_SHAPE_CLASS ')';

// parameter_value - from QIF (@xId, @asmPathId, @asmPathXId)
arithmetic_fn
	: variable
	| feature_parameter
	| characteristic_parameter
	| dme_parameter
	| part_parameter
	| feature_length
	| feature_area
	| feature_size
	| max
	| min
	| characteristic_tolerance
	| object_parameter
	;

variable: VARIABLE_KEYWORD '(' identifier ')';
feature_parameter: FEATURE_PARAMETER_KEYWORD '(' xpath (',' ENUM_FEATURE)? ')';
characteristic_parameter: CHARACTERISTIC_PARAMETER_KEYWORD '(' xpath (',' ENUM_CHARACTERISTIC)? ')';
dme_parameter: DME_PARAMETER_KEYWORD '(' xpath ',' ENUM_DME_CLASS ')';
part_parameter: PART_PARAMETER_KEYWORD '(' xpath ')';
feature_length: FEATURE_LENGTH_KEYWORD;
feature_area: FEATURE_AREA_KEYWORD;
feature_size: FEATURE_SIZE_KEYWORD;
max: MAX_KEYWORD '(' arithmetic_expr (',' arithmetic_expr)+ ')';
min: MIN_KEYWORD '(' arithmetic_expr (',' arithmetic_expr)+ ')';
characteristic_tolerance: CHARACTERISTIC_TOLERANCE_KEYWORD;
object_parameter: OBJECT_PARAMETER_KEYWORD '(' xpath ')';

interval_fn: closed_interval; // TODO left_open_interval, open_interval, right_open_interval

closed_interval: CLOSED_INTERVAL_KEYWORD '(' arithmetic_expr ',' arithmetic_expr ')';

xpath : xpath_component ('/' xpath_component)*;

xpath_component: identifier;

identifier
	: UUID_KEYWORD
    | IF_KEYWORD
	| NOT_KEYWORD
	| AND_KEYWORD
	| OR_KEYWORD
	| TOKEN_KEYWORD
	| CHARACTERISTIC_IS_KEYWORD
	| FEATURE_IS_DATUM_KEYWORD
	| FEATURE_IS_INTERNAL_KEYWORD
	| FEATURE_TYPE_IS_KEYWORD
	| SAMPLING_CATEGORY_IS_KEYWORD
	| SHAPE_CLASS_IS_KEYWORD
	| DME_THEN_DECISION_CLASS_KEYWORD
	| MUST_KEYWORD
	| MAY_KEYWORD
	| DME_THEN_ID_KEYWORD
	| WITH_KEYWORD
	| VARIABLE_KEYWORD
	| FEATURE_PARAMETER_KEYWORD
	| CHARACTERISTIC_PARAMETER_KEYWORD
	| DME_PARAMETER_KEYWORD
	| PART_PARAMETER_KEYWORD
	| FEATURE_LENGTH_KEYWORD
	| FEATURE_AREA_KEYWORD
	| FEATURE_SIZE_KEYWORD
	| MAX_KEYWORD
	| MIN_KEYWORD
	| CHARACTERISTIC_TOLERANCE_KEYWORD
	| OBJECT_PARAMETER_KEYWORD
	| DME_THEN_MAKE_MODEL_KEYWORD
	| MANUFACTURER_KEYWORD
	| MODEL_NUMBER_KEYWORD
	| SERIAL_NUMBER_KEYWORD
	| IN_KEYWORD
	| CLOSED_INTERVAL_KEYWORD
	| DME_RULE_KEYWORD
	| ENUM_CHARACTERISTIC
	| ENUM_FEATURE
	| ENUM_SHAPE_CLASS
	| ENUM_DME_CLASS
	| IDENTIFIER
	| BOOLEAN_TRUE_LITERAL
	| BOOLEAN_FALSE_LITERAL
	;

// Lexer Rules

UUID_KEYWORD : 'uuid';
IF_KEYWORD : 'if';
NOT_KEYWORD : 'not';
AND_KEYWORD : 'and';
OR_KEYWORD : 'or';
TOKEN_KEYWORD : 'token';
CHARACTERISTIC_IS_KEYWORD : 'characteristic_is';
FEATURE_IS_DATUM_KEYWORD : 'feature_is_datum';
FEATURE_IS_INTERNAL_KEYWORD : 'feature_is_internal';
FEATURE_TYPE_IS_KEYWORD : 'feature_type_is';
SAMPLING_CATEGORY_IS_KEYWORD : 'sampling_category_is';
SHAPE_CLASS_IS_KEYWORD : 'shape_class_is';
DME_THEN_DECISION_CLASS_KEYWORD: 'dme_class';
MUST_KEYWORD: 'must';
MAY_KEYWORD: 'may';
DME_THEN_ID_KEYWORD: 'dme_id';
WITH_KEYWORD: 'with';
VARIABLE_KEYWORD: 'variable';
FEATURE_PARAMETER_KEYWORD: 'feature_parameter';
CHARACTERISTIC_PARAMETER_KEYWORD: 'characteristic_parameter';
DME_PARAMETER_KEYWORD: 'dme_parameter';
PART_PARAMETER_KEYWORD: 'part_parameter';
FEATURE_LENGTH_KEYWORD: 'feature_length';
FEATURE_AREA_KEYWORD: 'feature_area';
FEATURE_SIZE_KEYWORD: 'feature_size';
MAX_KEYWORD: 'max';
MIN_KEYWORD: 'min';
CHARACTERISTIC_TOLERANCE_KEYWORD: 'characteristic_tolerance';
OBJECT_PARAMETER_KEYWORD: 'object_parameter';
DME_THEN_MAKE_MODEL_KEYWORD: 'dme_make_model';
MANUFACTURER_KEYWORD: 'manufacturer';
MODEL_NUMBER_KEYWORD: 'model_number';
SERIAL_NUMBER_KEYWORD: 'serial_number';
IN_KEYWORD: 'in';
CLOSED_INTERVAL_KEYWORD: 'closed_interval';
DME_RULE_KEYWORD: 'dme_rule';

// CharacteristicTypeEnumType
ENUM_CHARACTERISTIC
	: 'ANGLE'
	| 'ANGLECOORDINATE'
	| 'ANGLEFROM'
	| 'ANGLEBETWEEN'
	| 'ANGULARITY'
	| 'CHORD'
	| 'CIRCULARITY'
	| 'CIRCULARRUNOUT'
	| 'CONCENTRICITY'
	| 'CONICALTAPER'
	| 'CONICITY'
	| 'CURVELENGTH'
	| 'CYLINDRICITY'
	| 'DEPTH'
	| 'DIAMETER'
	| 'DISTANCE'
	| 'DISTANCEFROM'
	| 'ELLIPTICITY'
	| 'FLATTAPER'
	| 'FLATNESS'
	| 'HEIGHT'
	| 'LENGTH'
	| 'LENGTHCOORDINATE'
	| 'LINEPROFILE'
	| 'OTHERFORM'
	| 'PERPENDICULARITY'
	| 'PARALLELISM'
	| 'POINTPROFILE'
	| 'POSITION'
	| 'RADIUS'
	| 'SPHERICALDIAMETER'
	| 'SPHERICALRADIUS'
	| 'SPHERICITY'
	| 'SQUARE'
	| 'STRAIGHTNESS'
	| 'SURFACEPROFILE'
	| 'SURFACEPROFILENONUNIFORM'
	| 'SYMMETRY'
	| 'THICKNESS'
	| 'THREAD'
	| 'TOROIDICITY'
	| 'TOTALRUNOUT'
	| 'WELDBEVEL'
	| 'WELDEDGE'
	| 'WELDFILLET'
	| 'WELDFLAREBEVEL'
	| 'WELDFLAREV'
	| 'WELDJ'
	| 'WELDPLUG'
	| 'WELDSCARF'
	| 'WELDSEAM'
	| 'WELDSLOT'
	| 'WELDSPOT'
	| 'WELDSQUARE'
	| 'WELDSTUD'
	| 'WELDSURFACING'
	| 'WELDU'
	| 'WELDV'
	| 'WIDTH'
	;

// FeatureTypeEnumType
ENUM_FEATURE
    : 'CIRCLE'
    | 'CIRCULARARC'
    | 'CONE'
    | 'CONICALSEGMENT'
    | 'CYLINDER'
    | 'CYLINDRICALSEGMENT'
    | 'EDGEPOINT'
    | 'ELLIPSE'
    | 'ELLIPTICALARC'
    | 'ELONGATEDCIRCLE'
    | 'ELONGATEDCYLINDER'
    | 'EXTRUDEDCROSSSECTION'
    | 'GROUP'
    | 'LINE'
    | 'MARKING'
    | 'OPPOSITEANGLEDLINES'
    | 'OPPOSITEANGLEDPLANES'
    | 'OPPOSITEPARALLELLINES'
    | 'OPPOSITEPARALLELPLANES'
    | 'OTHERCURVE'
    | 'OTHERNONSHAPE'
    | 'OTHERSHAPE'
    | 'OTHERSURFACE'
    | 'PATTERN'
    | 'PATTERNCIRCLE'
    | 'PATTERNCIRCULARARC'
    | 'PATTERNLINEAR'
    | 'PATTERNPARALLELOGRAM'
    | 'PLANE'
    | 'POINT'
    | 'POINTDEFINEDCURVE'
    | 'POINTDEFINEDSURFACE'
    | 'SPHERE'
    | 'SPHERICALSEGMENT'
    | 'SURFACEOFREVOLUTION'
    | 'THREADED'
    | 'TOROIDALSEGMENT'
    | 'TORUS'
	;

// ShapeClassEnumType
ENUM_SHAPE_CLASS
	: 'GEAR'
    | 'FREEFORM'
    | 'PRISMATIC'
    | 'ROTATIONAL'
    | 'THINWALLED'
	;

// DMEClassNameEnumType
ENUM_DME_CLASS
	: 'AACMM'
	| 'ALLDMES'
	| 'ANALOG_MICROMETER'
	| 'AUTOCOLLIMATOR'
	| 'CALIPER'
	| 'CAPACITIVE_SENSOR'
	| 'CARTESIAN_CMM'
	| 'CHARGE_COUPLED_DEVICE_CAMERA_SENSOR'
	| 'CMM'
	| 'COMPLEX_TACTILE_PROBE_SENSOR'
	| 'COMPUTED_TOMOGRAPHY'
	| 'CONFOCAL_CHROMATIC_SENSOR'
	| 'DIAL_CALIPER'
	| 'DIGITAL_CALIPER'
	| 'DIGITAL_MICROMETER'
	| 'DRAW_WIRE_SENSOR'
	| 'DVRT_SENSOR'
	| 'EDDY_CURRENT_SENSOR'
	| 'GAGE'
	| 'LASER_RADAR'
	| 'LASER_TRACKER'
	| 'LASER_TRIANGULATION_SENSOR'
	| 'LIGHT_PEN_CMM'
	| 'LVDT_SENSOR'
	| 'MAGNETO_INDUCTIVE_SENSOR'
	| 'MEASUREMENT_ROOM'
	| 'MICROMETER'
	| 'MICROSCOPE'
	| 'MULTIPLE_CARRIAGE_CARTESIAN_CMM'
	| 'OPTICAL_COMPARATOR'
	| 'PARALLEL_LINK_CMM'
	| 'PROBE_TIP'
	| 'SIMPLE_TACTILE_PROBE_SENSOR'
	| 'SINE_BAR'
	| 'STRUCTURED_LIGHT_SENSOR'
	| 'TACTILE_PROBE_SENSOR'
	| 'THEODOLITE'
	| 'TOOL_WITH_CCD_CAMERA_SENSOR'
	| 'TOOL_WITH_CAPACITIVE_SENSOR'
	| 'TOOL_WITH_COMPLEX_TACTILE_PROBE_SENSOR'
	| 'TOOL_WITH_CONFOCAL_CHROMATIC_SENSOR'
	| 'TOOL_WITH_DETACHABLE_SENSORS'
	| 'TOOL_WITH_DVRT_SENSOR'
	| 'TOOL_WITH_DRAW_WIRE_SENSOR'
	| 'TOOL_WITH_EDDY_CURRENT_SENSOR'
	| 'TOOL_WITH_INTEGRATED_SENSOR'
	| 'TOOL_WITH_LVDT_SENSOR'
	| 'TOOL_WITH_LASER_TRIANGULATION_SENSOR'
	| 'TOOL_WITH_MAGNETOINDUCTIVE_SENSOR'
	| 'TOOL_WITH_SIMPLE_TACTILE_PROBE_SENSOR'
	| 'TOOL_WITH_STRUCTURED_LIGHT_SENSOR'
	| 'TOOL_WITH_ULTRASONIC_SENSOR'
	| 'ULTRASONIC_SENSOR'
	| 'UNIVERSAL_DEVICE'
	| 'UNIVERSAL_LENGTH_MEASURING'
	;

// "00001111-2222-3333-4444-555566667777"
UUID : HEX4 HEX4 '-' HEX4 '-' HEX4 '-' HEX4 '-' HEX4 HEX4 HEX4;
fragment HEX4 : [0-9a-fA-F] [0-9a-fA-F] [0-9a-fA-F] [0-9a-fA-F];

STRING_LITERAL : '"' (ESCAPED_SYMBOL | ~["\\])* '"' ;
fragment ESCAPED_SYMBOL : '\\' ["\\] ;

BOOLEAN_TRUE_LITERAL : 'true';
BOOLEAN_FALSE_LITERAL : 'false';

INTEGER : INT;

DOUBLE
    : INT '.' [0-9]+ EXP? // 1.35, 1.35E-9, 0.3
    | INT EXP             // 1e10
    ;
fragment EXP : [Ee] [+\-]? INT;
fragment INT : '0' | [1-9] [0-9]*; // no leading zeros

IDENTIFIER : [a-zA-Z_] [a-zA-Z_0-9]*;

COMMENT: '#' ~[\r\n]* -> skip;
WS : [ \t\r\n]+ -> skip;
