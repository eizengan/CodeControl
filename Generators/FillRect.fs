/*{
	"CREDIT": "VJ Codec",
	"DESCRIPTION": "Filled rectangle generator for CodeContol",
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
		    "NAME": "width",
		    "TYPE": "float",
			"MIN": 0.0,
			"DEFAULT": 0.0
	    },
	    {
		    "NAME": "height",
		    "TYPE": "float",
			"MIN": 0.0,
			"DEFAULT": 0.0
	    }
	]
}*/

void main() {
	vec4 aspect;
	aspect.zw = max(vec2(RENDERSIZE.x/RENDERSIZE.y, RENDERSIZE.y/RENDERSIZE.x), 1.0);
	aspect.st = isf_FragNormCoord.xy * aspect.zw;
	vec2 position = vec2(x, y);
	vec2 size = vec2(width, height);
	vec4 edges = vec4(      step(position - 0.5*size, aspect.xy),
	                  1.0 - step(position + 0.5*size, aspect.xy));
	gl_FragColor = vec4(vec3(1.0 - edges.x*edges.y*edges.z*edges.w), 1.0);
}