/*{
    "ISFVSN": "2.0",
    "CREDIT": "VJ Codec",
    "DESCRIPTION": "Mural-based palletizer for CodeControl",
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
            "NAME": "bw_size",
            "TYPE": "float",
            "MIN": 0.0
        },
        {
            "NAME": "bw_bands",
            "TYPE": "float",
            "MIN": 1.0,
			"DEFAULT": 1.0
        },
        {
            "NAME": "bw_balance",
            "TYPE": "float",
            "MIN": 0.0,
            "MAX": 1.0,
            "DEFAULT": 0.5
        }
    ]
}*/
vec3 rgb2hsv(vec3 c)
{
    vec4 K = vec4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
    vec4 p = mix(vec4(c.bg, K.wz), vec4(c.gb, K.xy), step(c.b, c.g));
    vec4 q = mix(vec4(p.xyw, c.r), vec4(c.r, p.yzx), step(p.x, c.r));

    float d = q.x - min(q.w, q.y);
    float e = 1.0e-10;
    return vec3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
}
vec3 hsv2rgb(vec3 c) {
    vec4 K = vec4(1.0, 0.666666667, 0.333333333, 3.0);
    vec3 p = abs(fract(c.xxx + K.xyz) * 6.0 - K.www);
    return c.z * mix(K.xxx, clamp(p - K.xxx, 0.0, 1.0), c.y);
}

void main() {
    vec4 pixel = IMG_THIS_PIXEL(inputImage);
    vec3 hsv = rgb2hsv(pixel.rgb);
	
    float index = pixel.a;
    index = (1.0 + bw_size) * index;
   
    float band_size_div = 0.0;
	if (bw_size != 0.0)
	    band_size_div = bw_bands / bw_size;   
   
    pixel.rgb = float(index<=1.0)  * hsv2rgb(vec3(index, 1.0, hsv.z)) +
                float(index> 1.0) * float(fract((index - 1.0)*band_size_div) < bw_balance);
	pixel.a = 1.0;
  
    gl_FragColor = pixel;
}