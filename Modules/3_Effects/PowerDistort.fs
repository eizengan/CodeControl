/*{
    "ISFVSN": "2.0",
    "CREDIT": "VJ Codec",
    "DESCRIPTION": "Power distortion effect",
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
            "NAME": "center_x",
            "TYPE": "float",
            "MAX": 2
        },
        {
            "NAME": "center_y",
            "TYPE": "float"
        },
        {
            "NAME": "radius",
            "TYPE": "float",
            "MIN": 0.1
        },
        {
            "NAME": "amount",
            "TYPE": "float",
            "MIN": -4,
            "MAX": 4,
            "DEFAULT": 0
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
	vec2 center = vec2(center_x, center_y);

	//warp
	vec2 pix_centered = aspect.xy - center;
	float rad_pct = length(pix_centered) / radius;
	vec2 st = (pow(rad_pct, amount)*pix_centered + center)/aspect.zw;

	//wrap
	vec2 rept = mod(st, 2.0);
	st = floor(rept) + sign(1.0 - rept)*fract(st);

    gl_FragColor = IMG_NORM_PIXEL(image, st);
}
