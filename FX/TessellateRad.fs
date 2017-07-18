/*{
    "ISFVSN": "2.0",
    "CREDIT": "VJ Codec",
    "DESCRIPTION": "Radial tessellation effect",
    "CATEGORIES": [
        "filter"
    ],
    "INPUTS": [
        {
            "NAME": "inputImage",
            "TYPE": "image"
        },
        {
            "NAME": "center_x",
            "TYPE": "float"
        },
        {
            "NAME": "center_y",
            "TYPE": "float"
        },
        {
            "NAME": "angle_offset",
            "TYPE": "float",
            "MIN": 0.0,
            "MAX": 1.0
        },
        {
            "NAME": "divisor",
            "TYPE": "float",
            "MIN": 1.0
        }
    ]
}*/

#define TAU 6.283185307

vec4 getAspect() {
    vec4 aspect;
    aspect.z = max(1.0, RENDERSIZE.x/RENDERSIZE.y);
    aspect.w = max(1.0, RENDERSIZE.y/RENDERSIZE.x);
    aspect.xy = aspect.zw * isf_FragNormCoord.xy;
    return aspect;
}

void main() {
    vec4 aspect = getAspect();
    
    vec2 center = vec2(center_x, center_y);
    float tessAngle = TAU/floor(divisor);
    
    vec2 location = aspect.xy - center;
    float ang = atan(location.y, location.x);
    float rad = length(location);
    ang = mod(ang, tessAngle) + angle_offset*TAU;
    
    vec2 tessLocation = rad*vec2(cos(ang), sin(ang));
    tessLocation += center;
    
    gl_FragColor = IMG_NORM_PIXEL(inputImage, tessLocation/aspect.zw);
}
