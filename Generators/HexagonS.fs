/*{
    "ISFVSN": "2.0",
    "CREDIT": "VJ Conduit",
    "DESCRIPTION": "Hexagon generator for CodeControl",
    "CATEGORIES": [
        "filter",
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
            "TYPE": "float"
        },
        {
            "NAME": "height",
            "TYPE": "float"
        },
        {
            "NAME": "center_height",
            "TYPE": "float",
            "MIN": 0.0,
            "MAX": 1.0,
            "DEFAULT": 0.5
        },
        {
            "NAME": "base_width",
            "TYPE": "float",
            "MIN": 0.0,
            "MAX": 1.0,
            "DEFAULT": 0.5
        },
        {
            "NAME": "thickness",
            "TYPE": "float",
            "MIN": 0.0,
            "DEFAULT": 0.0
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
float endpoint(vec4 aspect, vec2 p1, vec2 p2, float thickness) {
    return float(distance(aspect.xy, p1) <= 0.5*thickness ||
                 dot(p2 - p1, aspect.xy - p1) > 0.0);
}
float thickLineSeg(vec4 aspect, vec2 p1, vec2 p2, float thickness) {
    vec4 tp12 = thickLinePoints(p1, p2, thickness);
    vec4 tp21 = thickLinePoints(p2, p1, thickness);
   
    float ep12 = endpoint(aspect, p1, p2, thickness);
    float degenerate = float(p1 == p2)*ep12;
   
    float seg = degenerate +
                slice(aspect, tp12.xy, tp12.zw, true) *
                slice(aspect, tp21.xy, tp21.zw, true) *
                ep12 * endpoint(aspect, p2, p1, thickness);
    return clamp(seg, 0.0, 1.0);
}

void main() {
    vec4 aspect = getAspect();
   
    vec4 offsets;
    offsets.xy = 0.5*vec2(width);
    offsets.y *= base_width;
    offsets.zw = vec2(height);
    offsets.w *= center_height;
   
    vec2 v1 = vec2(base_x + offsets.y, base_y);
    vec2 v2 = vec2(base_x + offsets.x, base_y + offsets.w);
    vec2 v3 = vec2(base_x + offsets.y, base_y + offsets.z);
    vec2 v4 = vec2(base_x - offsets.y, base_y + offsets.z);
    vec2 v5 = vec2(base_x - offsets.x, base_y + offsets.w);
    vec2 v6 = vec2(base_x - offsets.y, base_y);
   
    float l12 = thickLineSeg(aspect, v1, v2, thickness);
    float l23 = thickLineSeg(aspect, v2, v3, thickness);
    float l34 = thickLineSeg(aspect, v3, v4, thickness);
    float l45 = thickLineSeg(aspect, v4, v5, thickness);
    float l56 = thickLineSeg(aspect, v5, v6, thickness);
    float l61 = thickLineSeg(aspect, v6, v1, thickness);
   
    vec3 color = vec3(min(1.0, l12+l23+l34+l45+l56+l61));
   
    gl_FragColor = vec4(color, 1.0);
}