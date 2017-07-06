/*{
	"CREDIT": "VJ Codec",
	"DESCRIPTION": "Circle generator for CodeControl",
	"CATEGORIES": [
		"generator",
		"CodeControl"
	],
	"INPUTS": [
	    {
		    "NAME": "x",
		    "TYPE": "float"
	    },
	    {
		    "NAME": "y",
		    "TYPE": "float"
	    },
	    {
		    "NAME": "radius",
		    "TYPE": "float",
			"MIN": 0.0,
			"DEFAULT": 0.0
	    },
	    {
		    "NAME": "thickness",
		    "TYPE": "float",
			"MIN": 0.0,
			"DEFAULT": 0.0
	    }
	]
}*/


void main() {
	vec4 aspect;
	aspect.zw = max(vec2(RENDERSIZE.x/RENDERSIZE.y, RENDERSIZE.y/RENDERSIZE.x), 1.0);
	aspect.xy = isf_FragNormCoord.xy * aspect.zw;
	float outer = 1.0 - float(length(aspect.xy - vec2(x, y)) > radius + 0.5*thickness);
	float inner = float(length(aspect.xy - vec2(x, y)) > radius - 0.5*thickness);
	gl_FragColor = vec4(vec3(1.0 - outer*inner), 1.0);
}