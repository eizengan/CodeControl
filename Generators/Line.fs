/*
  {
    "ISFVSN": "2.0",
    "CREDIT": "VJ Codec",
    "DESCRIPTION": "Thick line generator for CodeControl",
    "CATEGORIES": [
        "generator",
        "CodeControl"
    ],
    "INPUTS": [
        {
            "NAME": "x1",
            "TYPE": "float",
            "MAX": 2.0
        },
        {
            "NAME": "y1",
            "TYPE": "float"
        },
        {
            "NAME": "x2",
            "TYPE": "float",
            "MAX": 2.0
        },
        {
            "NAME": "y2",
            "TYPE": "float"
        },
        {
            "NAME": "thickness",
            "TYPE": "float",
            "MIN": 0.0,
            "DEFAULT":0.0
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
vec4 thickLinePoints(vec2 p1, vec2 p2, float thickness) {
    vec2 displacement = normalize(p2.yx - p1.yx);
    displacement.x = -displacement.x;
    return vec4(p1, p2) + 0.5*thickness*displacement.xyxy;
}
float thickLine(vec4 aspect, vec2 p1, vec2 p2, float thickness) {
    vec4 tp12 = thickLinePoints(p1, p2, thickness);
    vec4 tp21 = thickLinePoints(p2, p1, thickness);
    
    return slice(aspect, tp12.xy, tp12.zw, true) *
           slice(aspect, tp21.xy, tp21.zw, true);
}

void main() {
    vec4 aspect = getAspect();
    
    vec2 v1 = vec2(x1, y1);
    vec2 v2 = vec2(x2, y2);
    
    gl_FragColor = vec4(thickLine(aspect, v1, v2, thickness));
}