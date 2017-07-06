/*{
    "CREDIT": "VJ Codec",
    "DESCRIPTION": "Filled isosceles triangle generator for CodeControl",
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

vec4 getAspect() {
    vec4 aspect;
    aspect.z = max(RENDERSIZE.x/RENDERSIZE.y, 1.0);
    aspect.w = max(RENDERSIZE.y/RENDERSIZE.x, 1.0);
    aspect.xy = isf_FragNormCoord.xy * aspect.zw;
    return aspect;
}

float slice(vec4 aspect, vec2 p1, vec2 p2, bool top) {
    if (p2.x == p1.x) return float(!(top ^^ aspect.x < p1.x));
    vec2 base = p2.x > p1.x ? p1 : p2;
    float slope = (p2.y-p1.y) / (p2.x-p1.x);
    return float(!(top ^^ (aspect.y - base.y)>slope*(aspect.x - base.x)));
}

float fillIsoTri(vec4 aspect, vec2 position, vec2 size) {
    vec2 v1 = vec2(position.x-0.5*size.x, position.y-0.5*size.y);
    vec2 v2 = vec2(position.x+0.5*size.x, position.y-0.5*size.y);
    vec2 v3 = vec2(position.x, position.y+0.5*size.y);
   
    float base = slice(aspect, v1, v2, true);
    float left = slice(aspect, v1, v3, false);
    float right = slice(aspect, v2, v3, false);
   
    return base*left*right;
}

void main() {
    vec4 aspect = getAspect();
   
    vec2 position = vec2(x, y);
    vec2 size = vec2(width, height);
   
    float isoTriangle = 1.0 - fillIsoTri(aspect, position, size);
   
    gl_FragColor = vec4(vec3(isoTriangle), 1.0);
}