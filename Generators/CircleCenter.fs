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
            "MIN": 0.0
        },
        {
            "NAME": "thickness",
            "TYPE": "float",
            "MIN": 0.0
        }
    ]
}*/

vec4 getAspect() {
    vec4 aspect;
    aspect.z = max(1.0, RENDERSIZE.x/RENDERSIZE.y);
    aspect.w = max(1.0, RENDERSIZE.y/RENDERSIZE.x);
    aspect.xy = aspect.zw * isf_FragNormCoord.xy;
    return aspect;
}

void main() {
    vec4 aspect = getAspect();

    vec2 center = vec2(x, y);
    vec2 edges = radius + thickness * vec2(-0.5, 0.5);
    float pixelDistance = distance(aspect.xy, center);

    vec4 color = vec4(float(edges.s < pixelDistance && pixelDistance <= edges.t));

    gl_FragColor = color;
}
