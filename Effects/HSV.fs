/*{
    "ISFVSN": "2.0",
    "CREDIT": "VJ Codec",
    "DESCRIPTION": "HSV utilities for CodeControl",
    "CATEGORIES": [
        "utility",
        "CodeControl"
    ],
    "INPUTS": [
        {
            "NAME": "inputImage",
            "TYPE": "image"
        },
        {
            "NAME": "hue_offset",
            "TYPE": "float"
        },
        {
            "NAME": "sat_compression",
            "TYPE": "float"
        },
        {
            "NAME": "sat_adjust",
            "TYPE": "float",
            "MIN": -1.0,
            "MAX": 1.0
        },
        {
            "NAME": "val_compression",
            "TYPE": "float"
        },
        {
            "NAME": "val_adjust",
            "TYPE": "float",
            "MIN": -1.0,
            "MAX": 1.0
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
    
    float sc_scaled = float(sat_compression >= 0.0)*(1.0 + sat_compression) +
                      float(sat_compression <  0.0)*(1.0/(1.0 - sat_compression));
    float sat = pow(hsv.y, sc_scaled);
    float vc_scaled = float(val_compression >= 0.0)*(1.0 + val_compression) +
                      float(val_compression <  0.0)*(1.0/(1.0 - val_compression));
    float val = pow(hsv.z, vc_scaled);
   
   
    float hue = fract(hsv.x + hue_offset);
    sat = (1.0 + float(sat_adjust < 0.0)*sat_adjust) * sat +
                float(sat_adjust > 0.0)*mix(0.0, 1.0-sat, sat_adjust);
    val = (1.0 + float(val_adjust < 0.0)*val_adjust) * val +
                float(val_adjust > 0.0)*mix(0.0, 1.0-val, val_adjust);
                
    pixel.rgb = hsv2rgb(vec3(hue, sat, val));
    gl_FragColor = pixel;
}