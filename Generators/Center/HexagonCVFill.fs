/*{
    "ISFVSN": "2.0",
    "CREDIT": "VJ Conduit",
    "DESCRIPTION": "Filled hexagon generator for CodeControl",
    "CATEGORIES": [
        "filter",
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
        },
        {
            "NAME": "center_height",
            "TYPE": "float",
            "MIN": 0.0,
            "MAX": 1.0,
            "DEFAULT": 0.5
        },
        {
            "NAME": "center_width",
            "TYPE": "float",
            "MIN": 0.0,
            "MAX": 1.0,
            "DEFAULT": 0.5
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

    float base_y = y - 0.5*height;
    float y_center = center_height * height;
    vec4 offsets;
    offsets.xy = width * vec2(-0.5, 0.5);             //x offsets
    offsets.zw = vec2(-y_center, height - y_center);  //y offsets
    offsets.zw = y_center + center_width * offsets.zw;

    vec2 v1 = vec2(x,             base_y);
    vec2 v2 = vec2(x + offsets.y, base_y + offsets.z);
    vec2 v3 = vec2(x + offsets.y, base_y +  offsets.w);
    vec2 v4 = vec2(x,             base_y + height);
    vec2 v5 = vec2(x + offsets.x, base_y + offsets.w);
    vec2 v6 = vec2(x + offsets.x, base_y + offsets.z);

    float s12 = slice(aspect, v1, v2, false);
    float s23 = slice(aspect, v2, v3, false);
    float s34 = slice(aspect, v3, v4, false);
    float s45 = slice(aspect, v4, v5, false);
    float s56 = slice(aspect, v5, v6, false);
    float s61 = slice(aspect, v6, v1, false);

    vec4 color = vec4(s12*s23*s34*s45*s56*s61);

    gl_FragColor = color;
}