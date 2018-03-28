/*{
    "ISFVSN": "2.0",
    "CREDIT": "VJ Codec",
    "DESCRIPTION": "Luminance-based indexer for CodeControl",
    "CATEGORIES": [
        "indexer",
        "CodeControl"
    ],
    "INPUTS": [
        {
            "NAME": "inputImage",
            "TYPE": "image"
        },
        {
            "NAME": "mirror",
            "TYPE": "bool"
        },
        {
            "NAME": "multiplier",
            "TYPE": "float"
        },
        {
            "NAME": "offset",
            "TYPE": "float"
        },
        {
            "NAME": "compression",
            "TYPE": "float"
        }
    ]
}*/

void main() {
    vec4 pixel = IMG_THIS_PIXEL(inputImage);
    
    float a = pixel.a;
    a = multiplier * a + offset;
    float mirror_a = mod(2.0*a, 2.0);
    mirror_a = float(mirror_a <= 1.0)*mirror_a +
               float(mirror_a >  1.0)*(2.0 - mirror_a);
    a = float( mirror)*mirror_a +
        float(!mirror)*fract(a);
    
    float comp_scaled;
    if (compression >= 0.0)
        comp_scaled = 1.0 + compression;
    else
        comp_scaled = 1.0/(1.0 - compression);

    pixel.a *= pow(a, comp_scaled);
    gl_FragColor = pixel;
}
