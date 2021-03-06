/*{
    "ISFVSN": "2.0",
    "CREDIT": "VJ Codec",
    "DESCRIPTION": "Filled circle generator for CodeControl",
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
            "NAME": "radius",
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

    vec2 center = vec2(base_x, base_y + radius);
    float pixelDistance = distance(aspect.xy, center);

    vec4 color = vec4(float(pixelDistance <= radius));

    gl_FragColor = color;
}
