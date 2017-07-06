/*{
    "CREDIT": "VJ Codec",
    "DESCRIPTION": "Filled triangle generator for CodeControl",
    "CATEGORIES": [
        "generator",
		"CodeControl"
    ],
    "INPUTS": [
        {
            "NAME": "base_x",
            "TYPE": "float"
        },
        {
            "NAME": "base_y",
            "TYPE": "float"
        },
        {
            "NAME": "base_width",
            "TYPE": "float"
        },
        {
            "NAME": "vertex_x",
            "TYPE": "float"
        },
        {
            "NAME": "vertex_y",
            "TYPE": "float"
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

float fillTri(vec4 aspect, vec2 basePosition, float baseWidth, vec2 vertexPosition) {
    bool pointsUp = vertexPosition.y > basePosition.y;
   
    vec2 v1 = vec2(basePosition.x - 0.5*baseWidth, basePosition.y);
    vec2 v2 = vec2(basePosition.x + 0.5*baseWidth, basePosition.y);
    vec2 v3 = vec2(vertexPosition.x, vertexPosition.y);
   
    float base = slice(aspect, v1, v2, pointsUp);
    float left = slice(aspect, v1, v3, !(pointsUp ^^ vertexPosition.x < v1.x));
    float right = slice(aspect, v2, v3, !(pointsUp ^^ vertexPosition.x > v2.x));
   
    return base*left*right;
}

void main() {
    vec4 aspect = getAspect();
    vec2 basePosition = vec2(base_x, base_y);
    vec2 vertexPosition = vec2(vertex_x, vertex_y);
   
    float triangle = 1.0 - fillTri(aspect, basePosition, base_width, vertexPosition);
   
    gl_FragColor = vec4(vec3(triangle), 1.0);
}