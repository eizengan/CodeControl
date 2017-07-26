/*{
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
            "MIN": 0.0,
            "DEFAULT": 0.0
        },
        {
            "NAME": "height",
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

void main() {
    vec4 aspect = getAspect();
    
    vec2 position = vec2(x, y);
    vec2 size = vec2(width, height);
    
    vec4 edges = vec4(      step(position - 0.5*size, aspect.xy),
                      1.0 - step(position + 0.5*size, aspect.xy));
                      
    gl_FragColor = vec4(edges.x*edges.y*edges.z*edges.w);
}