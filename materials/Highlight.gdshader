shader_type spatial;
render_mode unshaded, cull_disabled, depth_prepass_alpha;

uniform sampler2D texture_albedo : source_color;
uniform vec4 outline_color : source_color = vec4(0.0, 0.0, 0.0, 1.0);
uniform float outline_thickness = 1.0;
uniform float pixel_size = 1.0;
uniform float outline_depth_offset = -0.001; // pushes outline slightly backward

void fragment() {
    vec2 tex_size = vec2(textureSize(texture_albedo, 0));
    vec2 uv = UV;

    // Pixelation effect
    uv = floor(uv * tex_size / pixel_size) * pixel_size / tex_size;
    vec4 base = texture(texture_albedo, uv);

    // Outline detection
    float px = outline_thickness / tex_size.x;
    float py = outline_thickness / tex_size.y;
    bool edge = false;

    if (base.a <= 0.1) {
        for (int x = -1; x <= 1; x++) {
            for (int y = -1; y <= 1; y++) {
                vec2 offset = vec2(float(x) * px, float(y) * py);
                if (texture(texture_albedo, uv + offset).a > 0.1) {
                    edge = true;
                }
            }
        }
    }

    // Draw outline slightly behind
    if (edge) {
        ALBEDO = outline_color.rgb;
        ALPHA = outline_color.a;
        VERTEX += NORMAL * outline_depth_offset;
    } else if (base.a > 0.1) {
        // Draw main sprite normally
        ALBEDO = base.rgb;
        ALPHA = base.a;
    } else {
        discard;
    }
}
