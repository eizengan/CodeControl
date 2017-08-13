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
            "DEFAULT": true,
            "IDENTITY": true
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
            "IDENTITY": 0.0
        },
        {
            "NAME": "angle_offset",
            "TYPE": "float",
            "IDENTITY": 0.0
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
    vec4 pixel = float(show_original)*IMG_THIS_PIXEL(inputImage);
    for (int iii = 0; iii < 6; ++iii) {
        if (iii > type) break;
        float ang = float(iii)*incr + angle_offset*TAU;
        vec2 st = amount*vec2(cos(ang), sin(ang));
        st = aspSizeInv*(aspect.st + st);
        vec4 image = IMG_NORM_PIXEL(inputImage, st);
        float a = image.a + pixel.a*(1.0 - image.a);
        if (a == 0.0)
            pixel.rgb = vec3(0.0);
        else
            pixel.rgb = (image.rgb*image.a + pixel.rgb*pixel.a*(1.0 - image.a))/a;
        pixel.a = a;
    }
    gl_FragColor = pixel;
}
