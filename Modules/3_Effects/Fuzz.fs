/*{
    "ISFVSN": "2.0",
    "CREDIT": "VJ Codec",
    "DESCRIPTION": "Fuzz effect",
    "CATEGORIES": [
        "effect",
        "CodeControl"
    ],
    "INPUTS": [
        {
            "NAME": "image",
            "TYPE": "image"
        },
        {
            "NAME": "amount",
            "TYPE": "float",
            "MIN": 0.0
        }
    ]
}*/

precision highp float;

#define G 23.14069263277926
#define GS 2.665144142690225
#define TI 0.159154943092

void main() {
    float r = fract(cos(dot(gl_FragCoord.xy, vec2(G, GS))) * TIME);
    vec2 offset = 2.0*vec2(fract(r + TI), r) - 1.0;
    offset *= amount;
    gl_FragColor = IMG_PIXEL(image, gl_FragCoord.xy + offset);
}
