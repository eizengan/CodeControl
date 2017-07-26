/*{
    "CREDIT": "VJ Codec",
    "DESCRIPTION": "Splay effect for CodeContol",
    "CATEGORIES": [
        "effect",
        "CodeControl"
    ],
    "INPUTS": [
        {
            "NAME": "inputImage",
            "TYPE": "image"
        },
        {
            "NAME": "show_original",
            "TYPE": "bool",
            "DEFAULT": true
        },
        {
            "NAME": "type",
            "TYPE": "long",
            "VALUES": [2, 3, 4, 6],
            "LABELS": ["Bi","Tri","Quad","Hex"],
            "DEFAULT": 2
        },
        {
            "NAME": "amount",
            "TYPE": "float",
            "MIN": 0.0,
            "DEFAULT": 0.0
        },
        {
            "NAME": "angle_offset",
            "TYPE": "float",
            "MIN": 0.0,
            "DEFAULT": 0.0
        },
        {
            "NAME": "mix_mode",
            "TYPE": "long",
            "VALUES": [0, 1],
            "LABELS": ["Add", "Mult"],
            "DEFAULT": 0
        }
    ]
}*/

#define TAU    6.28318530718

vec4 getAspect() {
    vec4 aspect;
    aspect.zw = max(vec2(RENDERSIZE.x/RENDERSIZE.y, RENDERSIZE.y/RENDERSIZE.x), 1.0);
    aspect.st = isf_FragNormCoord.xy * aspect.zw;
    return aspect;
}

void main() {
    vec4 aspect = getAspect();
    vec2 aspSizeInv = 1.0 / aspect.zw;
    float incr = TAU / float(type);
    vec3 color = show_original ? IMG_THIS_PIXEL(inputImage).rgb : vec3(float(mix_mode));
    for (int iii = 0; iii < 6; ++iii) {
        if (iii > type) break;
        float ang = float(iii)*incr + angle_offset*TAU;
        vec2 st = amount*vec2(cos(ang), sin(ang));
        st = aspSizeInv*(aspect.st + st);
        vec3 image = IMG_NORM_PIXEL(inputImage, st).rgb;
        color = float(mix_mode)*color*image +
                (1.0 - float(mix_mode))*min(color + image, 1.0);
    }
    gl_FragColor = vec4(color, 1.0);
}