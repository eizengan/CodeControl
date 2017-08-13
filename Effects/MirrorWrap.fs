/*{
    "ISFVSN": "2.0",
    "CREDIT": "VJ Conduit",
    "DESCRIPTION": "Mirrored, wrapped scaling effect",
    "CATEGORIES": [
        "filter"
    ],
    "INPUTS": [
        {
            "NAME": "inputImage",
            "TYPE": "image"
        },
        {
            "NAME": "offset_x",
            "TYPE": "float",
            "IDENTITY": 0.0
        },
        {
            "NAME": "offset_y",
            "TYPE": "float",
            "IDENTITY": 0.0
        },
        {
            "NAME": "scale",
            "TYPE": "float",
            "DEFAULT": 1.0,
            "IDENTITY": 1.0
        }
    ]
}*/

void main() {
    vec2 offset = vec2(offset_x, offset_y) + 0.5;
    vec2 scaledCoord = scale*isf_FragNormCoord.xy - 0.5*scale + offset;
    vec2 rept = mod(scaledCoord, 2.0);
    gl_FragColor = IMG_NORM_PIXEL(inputImage, floor(rept) + sign(1.0 - rept)*fract(scaledCoord));
}
