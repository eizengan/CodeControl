/*{
    "ISFVSN": "2.0",
    "CREDIT": "VJ Codec",
    "DESCRIPTION": "Cartesian tesselation effect",
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
            "TYPE": "float"
        },
        {
            "NAME": "offset_y",
            "TYPE": "float"
        },
        {
            "NAME": "width",
            "TYPE": "float"
        },
        {
            "NAME": "height",
            "TYPE": "float"
        },
        {
            "NAME": "direction",
            "TYPE": "long",
            "VALUES": [0, 1, 2],
            "LABELS": ["Both", "X", "Y"]
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
    vec2 offset = vec2(offset_x, offset_y);
    vec2 size = vec2(width, height);
    
    vec2 sampleLocation = mod(aspect.xy - offset, size);
    
    vec4 both = IMG_NORM_PIXEL(inputImage, (sampleLocation + offset)/aspect.zw);
    vec4 horizontal = step(offset.y, aspect.y)*(1.0 - step(offset.y + height, aspect.y))*both;
    vec4 vertical = step(offset.x, aspect.x)*(1.0 - step(offset.x + width, aspect.x))*both;
    
    vec4 pixel = float(direction == 0) * both +
                 float(direction == 1) * horizontal +
                 float(direction == 2) * vertical;
    
    gl_FragColor = pixel;
}
