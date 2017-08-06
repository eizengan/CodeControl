/*{
    "CREDIT": "VJ Codec",
    "DESCRIPTION": "Faded circle generator for CodeControl",
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
        },
        {
            "NAME": "intensity",
            "TYPE": "float",
            "MIN": 0.0
        },
        {
            "NAME": "fade_out",
            "TYPE": "bool",
            "DEFAULT": true
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
    vec2 edges = radius + thickness * vec2(0.0, 1.0);
    float pixelDistance = distance(aspect.xy, center);
    float fadeDist = (pixelDistance - radius)/thickness;
    fadeDist = float(fade_out)*fadeDist + (1.0 - float(fade_out))*(1.0 - fadeDist);

    vec4 color;
    color.rgb = vec3(float(edges.s < pixelDistance && pixelDistance <= edges.t));
    color.a = color.r*(1.0 - pow(fadeDist, intensity));

    gl_FragColor = color;
}
