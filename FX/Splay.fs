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
            "NAME": "showOriginal",
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
        }
    ]
}*/

#define TAU    6.28318530718
#define OFFSET 1.57079632679

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
    vec3 color = showOriginal ? IMG_THIS_PIXEL(inputImage).rgb : vec3(1.0);
    for (int iii = 0; iii < 6; ++iii) {
        if (iii > type) break;
        float ang = float(iii)*incr + OFFSET;
        vec2 st = amount*vec2(cos(ang), sin(ang));
        st = aspSizeInv*(aspect.st + st);
        color *= IMG_NORM_PIXEL(inputImage, st).rgb;
    }
    gl_FragColor = vec4(color, 1.0);
}