shader_type spatial;
render_mode unshaded, cull_disabled;

const int MAX_EYES = 1024;


uniform sampler2D eye0;
uniform sampler2D eye1;
uniform sampler2D eye2;
uniform sampler2D eye3;

uniform int eye_count : hint_range(0, MAX_EYES) = 30;
uniform float seed = 12.34;
uniform vec3 eye_tint = vec3(1.0, 0.1, 0.1);
uniform float eye_radius = 0.03;
uniform float time : hint_range(0,1000) = 0.0;

// variável interpolada entre vertex e fragment
varying vec3 world_dir;

float rand(vec2 co) {
    return fract(sin(dot(co.xy, vec2(12.9898,78.233))) * 43758.5453);
}

vec2 spherical_to_uv(vec3 dir) {
    float u = atan(dir.z, dir.x) / (2.0 * PI) + 0.5;
    float v = asin(dir.y) / PI + 0.5;
    return vec2(u, v);
}

vec4 sample_eye_tex(int idx, vec2 uv) {
    if (idx == 0) return texture(eye0, uv);
    if (idx == 1) return texture(eye1, uv);
    if (idx == 2) return texture(eye2, uv);
    return texture(eye3, uv);
}

void vertex() {
    // converte posição do vértice para espaço de mundo
    vec3 world_pos = (MODEL_MATRIX * vec4(VERTEX, 1.0)).xyz;
    // posição da câmera no mundo
    vec3 cam_pos = (INV_VIEW_MATRIX[3]).xyz;
    // direção do ponto em relação à câmera
    world_dir = normalize(world_pos - cam_pos);
}

void fragment() {
    vec3 dir = normalize(world_dir);
    vec2 uv = spherical_to_uv(dir);
    vec3 accum = vec3(0.0);

    for (int i = 0; i < MAX_EYES; i++) {
        if (i >= eye_count) continue;
        float fi = float(i);
        vec2 pos = vec2(rand(vec2(fi, seed)), rand(vec2(seed, fi)));

        vec2 diff = uv - pos;
        diff = abs(diff);
        diff = min(diff, vec2(1.0) - diff);
        float d = length(diff);

        if (d < eye_radius) {
            vec2 eye_uv = d / eye_radius + vec2(0.5);
            int tex_index = int(mod(fi, 4.0));
            vec4 eye_tex = sample_eye_tex(tex_index, eye_uv);
            float flick = 0.8 + 0.2 * sin(time * 6.0 + fi * 0.37);
            vec3 col = eye_tint * eye_tex.rgb * eye_tex.a * flick;
            accum += col * eye_tex.a;
        }
    }

    ALBEDO = accum;
}
