/*{
    "ISFVSN": "2.0",
    "CREDIT": "VJ Codec",
    "DESCRIPTION": "Filled rectangle generator for CodeContol",
    "CATEGORIES": [
        "generator",
        "CodeControl"
    ],
    "INPUTS": [
        {
            "NAME": "x",
            "TYPE": "float"
        },
        {
            "NAME": "y",
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

    vec4 offsets;
    offsets.xy = 0.5 * vec2(-width, width);    //x offsets
    offsets.zw = vec2(0.0, height);  //y offsets

    vec2 v1 = vec2(x + offsets.x, y + offsets.z);
    vec2 v2 = vec2(x + offsets.x, y + offsets.w);
    vec2 v3 = vec2(x + offsets.y, y + offsets.w);
    vec2 v4 = vec2(x + offsets.y, y + offsets.z);

    float s12 = slice(aspect, v1, v2, true);
    float s23 = slice(aspect, v2, v3, true);
    float s34 = slice(aspect, v3, v4, true);
    float s41 = slice(aspect, v4, v1, true);

    vec4 color = vec4(s12*s23*s34*s41);

    gl_FragColor = color;
}
