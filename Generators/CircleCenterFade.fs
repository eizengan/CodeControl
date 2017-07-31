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
    
    float len = length(aspect.xy - center);
    vec4 color;
    bool innerCut = len > radius;
    float fadeDist = (len - radius)/thickness;
    fadeDist = float(fade_out)*fadeDist + (1.0 - float(fade_out))*(1.0 - fadeDist);
    color.rgb = vec3(float(innerCut && len < radius + thickness));
    color.a = float(innerCut)*(1.0 - pow(fadeDist, intensity));
    
    gl_FragColor = color;
}
