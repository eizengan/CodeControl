/*{
	"CREDIT": "VJ Codec",
	"DESCRIPTION": "Rectangle generator for CodeContol",
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
	aspect.st = isf_FragNormCoord.xy * aspect.zw;
	vec2 position = vec2(x, y);
	vec2 size = vec2(width, height);
	vec2 thickVec = vec2(0.5*thickness);
	vec4 edgePos = position.xyxy + vec4(-0.5*size, 0.5*size);
	vec4 outer = vec4(      step(edgePos.xy - thickVec, aspect.xy),
	                  1.0 - step(edgePos.zw + thickVec, aspect.xy));
	vec4 inner = vec4(      step(edgePos.xy + thickVec, aspect.xy),
	                  1.0 - step(edgePos.zw - thickVec, aspect.xy));
	gl_FragColor = vec4(vec3(1.0 - outer.x*outer.y*outer.z*outer.w + inner.x*inner.y*inner.z*inner.w), 1.0);
}