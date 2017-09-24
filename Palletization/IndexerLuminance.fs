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
    pixel.rgb = pixel.rgb*pixel.a;
    
    float index = 0.2989*pixel.r + 0.5866*pixel.g + 0.1145*pixel.b;
    index = multiplier * index + offset;
    float mirror_index = mod(2.0*index, 2.0);
    mirror_index = float(mirror_index <= 1.0)*mirror_index +
                   float(mirror_index >  1.0)*(2.0 - mirror_index);
    index = float( mirror)*mirror_index +
            float(!mirror)*fract(index);
    
    float comp_scaled;
    if (compression >= 0.0)
	    comp_scaled = 1.0 + compression;
	else
	    comp_scaled = 1.0/(1.0 - compression);
		
    pixel.a = pow(index, comp_scaled);    
    gl_FragColor = pixel;
}