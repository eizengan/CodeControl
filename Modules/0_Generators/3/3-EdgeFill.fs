/*{
    "ISFVSN": "2.0",
    "CREDIT": "VJ Codec",
    "DESCRIPTION": "Filled triangle generator for CodeControl",
    "CATEGORIES": [
        "generator",
        "CodeControl"
    ],
    "INPUTS": [
        {
            "NAME": "base_x",
            "TYPE": "float"
        },
        {
            "NAME": "base_y",
            "TYPE": "float"
        },
        {
            "NAME": "width",
            "TYPE": "float",
            "MIN": 0.0
        },
        {
            "NAME": "height",
            "TYPE": "float",
            "MIN": 0.0
        },
        {
            "NAME": "vertex_x",
            "TYPE": "float",
            "MIN": 0.0,
            "MAX": 1.0
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
float slice(vec4 aspect, vec2 p1, vec2 p2, bool clockwise) {
    vec2 points = p2 - p1;
    vec2 pix = aspect.yx - p1.yx;
    pix.x = -pix.x;
    float inverter = 2.0*float(clockwise) - 1.0;
    return float(p1 != p2)*step(0.0, inverter*dot(points, pix));
}

void main() {
    vec4 aspect = getAspect();

    vec2 v1 = vec2(base_x - 0.5*width, base_y);
    vec2 v2 = vec2(base_x + 0.5*width, base_y);
    vec2 v3 = vec2(base_x + (vertex_x - 0.5)*width, base_y + height);

    float s12 = slice(aspect, v1, v2, false);
    float s23 = slice(aspect, v2, v3, false);
    float s31 = slice(aspect, v3, v1, false);

    vec4 color = vec4(s12*s23*s31);

    gl_FragColor = color;
}
