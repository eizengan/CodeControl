/*{
    "ISFVSN": "2.0",
    "CREDIT": "VJ Codec",
    "DESCRIPTION": "Diagnostic palletizer for CodeControl",
    "CATEGORIES": [
        "palletizer",
        "CodeControl"
    ],
    "INPUTS": [
        {
            "NAME": "inputImage",
            "TYPE": "image"
        },
        {
            "NAME": "greyscale",
            "TYPE": "bool"
        }
    ]
}*/

void main() {
    vec4 pixel = IMG_THIS_PIXEL(inputImage);
    pixel = mix(vec4(pixel.a * pixel.rgb, 1.0), vec4(pixel.aaa, 1.0), float(greyscale));
    gl_FragColor = pixel;
}