#!/usr/bin/env python3
"""
T-001 Year One Gameplay Flow GIF Generator
Renders a 960x540 animated GIF illustrating all 12 key milestones of Year 1 gameplay flow.
"""

import os
from PIL import Image, ImageDraw, ImageFont

BASE_DIR = os.path.abspath(os.path.join(os.path.dirname(__file__), '../../mobile-2d/Assets/Sprites/MVP'))
OUTPUT_DIR = os.path.dirname(os.path.abspath(__file__))

W, H = 960, 540

# Fonts
FONT_REGULAR_PATH = "/Library/Fonts/Arial Unicode.ttf"
if not os.path.exists(FONT_REGULAR_PATH):
    FONT_REGULAR_PATH = "/System/Library/Fonts/Supplemental/Arial.ttf"

font_title = ImageFont.truetype(FONT_REGULAR_PATH, 20)
font_subtitle = ImageFont.truetype(FONT_REGULAR_PATH, 14)
font_body = ImageFont.truetype(FONT_REGULAR_PATH, 16)
font_small = ImageFont.truetype(FONT_REGULAR_PATH, 13)
font_badge = ImageFont.truetype(FONT_REGULAR_PATH, 13)

def load_img(rel_path):
    p = os.path.join(BASE_DIR, rel_path)
    if not os.path.exists(p):
        print(f"Warning: file not found: {p}")
        return None
    return Image.open(p).convert("RGBA")

def scale_bg(bg_img, target_w=W, target_h=H):
    img_w, img_h = bg_img.size
    ratio = max(target_w / img_w, target_h / img_h)
    new_w, new_h = int(img_w * ratio), int(img_h * ratio)
    resized = bg_img.resize((new_w, new_h), Image.Resampling.LANCZOS)
    left = (new_w - target_w) // 2
    top = (new_h - target_h) // 2
    return resized.crop((left, top, left + target_w, top + target_h))

def place_sprite(canvas, sprite, x, y, scale=1.0, flip_x=False):
    if sprite is None:
        return
    s = sprite
    if scale != 1.0:
        nw, nh = int(s.width * scale), int(s.height * scale)
        s = s.resize((nw, nh), Image.Resampling.LANCZOS)
    if flip_x:
        s = s.transpose(Image.FLIP_LEFT_RIGHT)
    canvas.alpha_composite(s, (int(x), int(y)))

def draw_hud(canvas, title, milestone_tag, time_tag=""):
    overlay = Image.new("RGBA", (W, H), (0, 0, 0, 0))
    draw = ImageDraw.Draw(overlay)
    
    # Top bar
    draw.rectangle([0, 0, W, 48], fill=(16, 22, 34, 235))
    draw.line([(0, 48), (W, 48)], fill=(0, 205, 255, 220), width=2)
    
    # Milestone pill
    draw.rounded_rectangle([14, 9, 155, 39], radius=6, fill=(30, 70, 125, 240), outline=(75, 150, 245, 255), width=1)
    draw.text((24, 14), milestone_tag, font=font_subtitle, fill=(230, 245, 255))
    
    # Title
    draw.text((170, 12), title, font=font_title, fill=(255, 255, 255))
    
    # Time / Status tag
    if time_tag:
        bbox = draw.textbbox((0, 0), time_tag, font=font_subtitle)
        tw = bbox[2] - bbox[0]
        rx = W - tw - 30
        draw.rounded_rectangle([rx, 9, rx + tw + 18, 39], radius=6, fill=(18, 52, 40, 240), outline=(45, 215, 115, 255), width=1)
        draw.text((rx + 9, 14), time_tag, font=font_subtitle, fill=(110, 255, 175))
        
    canvas.alpha_composite(overlay)

def draw_caption_card(canvas, badge_text, badge_color, line1, line2=""):
    overlay = Image.new("RGBA", (W, H), (0, 0, 0, 0))
    draw = ImageDraw.Draw(overlay)
    
    # Bottom card
    card_x0, card_y0 = 20, 432
    card_x1, card_y1 = W - 20, 525
    draw.rounded_rectangle([card_x0, card_y0, card_x1, card_y1], radius=10, fill=(10, 16, 26, 235), outline=(65, 105, 155, 220), width=2)
    
    # Badge
    bg_col, border_col, text_col = badge_color
    bbox = draw.textbbox((0, 0), badge_text, font=font_badge)
    bw = bbox[2] - bbox[0] + 16
    draw.rounded_rectangle([card_x0 + 15, card_y0 + 11, card_x0 + 15 + bw, card_y0 + 35], radius=5, fill=bg_col, outline=border_col, width=1)
    draw.text((card_x0 + 23, card_y0 + 13), badge_text, font=font_badge, fill=text_col)
    
    # Content line 1
    draw.text((card_x0 + 25 + bw, card_y0 + 12), line1, font=font_body, fill=(255, 255, 255))
    
    # Content line 2
    if line2:
        draw.text((card_x0 + 20, card_y0 + 48), line2, font=font_small, fill=(185, 215, 245))
        
    canvas.alpha_composite(overlay)

def draw_choice_box(canvas, title, option_a, option_b, selected=None):
    overlay = Image.new("RGBA", (W, H), (0, 0, 0, 0))
    draw = ImageDraw.Draw(overlay)
    bx0, by0, bx1, by1 = 280, 120, 680, 250
    draw.rounded_rectangle([bx0, by0, bx1, by1], radius=12, fill=(15, 22, 35, 240), outline=(240, 180, 50, 240), width=2)
    draw.text((bx0 + 20, by0 + 12), title, font=font_body, fill=(255, 215, 80))
    
    # Option A
    col_a_fill = (40, 80, 140, 250) if selected == 'A' else (25, 35, 55, 200)
    col_a_border = (100, 180, 255, 255) if selected == 'A' else (60, 80, 110, 180)
    draw.rounded_rectangle([bx0 + 20, by0 + 45, bx1 - 20, by0 + 82], radius=6, fill=col_a_fill, outline=col_a_border, width=2 if selected == 'A' else 1)
    marker_a = "[✓] " if selected == 'A' else "    "
    draw.text((bx0 + 30, by0 + 52), f"{marker_a}A. {option_a}", font=font_small, fill=(255, 255, 255))
    
    # Option B
    col_b_fill = (40, 80, 140, 250) if selected == 'B' else (25, 35, 55, 200)
    col_b_border = (100, 180, 255, 255) if selected == 'B' else (60, 80, 110, 180)
    draw.rounded_rectangle([bx0 + 20, by0 + 88, bx1 - 20, by0 + 125], radius=6, fill=col_b_fill, outline=col_b_border, width=2 if selected == 'B' else 1)
    marker_b = "[✓] " if selected == 'B' else "    "
    draw.text((bx0 + 30, by0 + 95), f"{marker_b}B. {option_b}", font=font_small, fill=(255, 255, 255))
    
    canvas.alpha_composite(overlay)

# Badge presets
BADGE_EVENT = ((25, 60, 110, 255), (60, 140, 240, 255), (200, 230, 255))
BADGE_CHOICE = ((100, 70, 15, 255), (240, 170, 30, 255), (255, 240, 180))
BADGE_ALERT = ((110, 30, 30, 255), (245, 70, 70, 255), (255, 215, 215))
BADGE_SUCCESS = ((20, 80, 45, 255), (45, 205, 110, 255), (200, 255, 220))
BADGE_DASHBOARD = ((60, 30, 105, 255), (160, 85, 245, 255), (235, 215, 255))
BADGE_ROUTINE = ((40, 60, 75, 255), (80, 145, 175, 255), (220, 245, 255))

print("Loading cached assets...")
bg_reception = scale_bg(load_img('remaining/E00/dorm_reception_background.png'))
bg_hallway = scale_bg(load_img('dorm_journey/hallway.png'))
bg_room404 = scale_bg(load_img('environment/room_404_backdrop.png'))
bg_canteen = scale_bg(load_img('remaining/E01/canteen_background.png'))
bg_wc = scale_bg(load_img('dorm_journey/wc_male.png'))
bg_campus = scale_bg(load_img('commute/campus.png'))
bg_classroom = scale_bg(load_img('year1/h6_classroom_v2.png'))

npc_manager = load_img('remaining/C00/npc_dorm_manager.png')
luon_backpack_idle = load_img('remaining/AN01/luon_backpack_front_idle.png')
luon_backpack_walk = load_img('remaining/AN01/luon_backpack_front_walk_1.png')
keys_prop = load_img('remaining/P01/keys_generic.png')
bunk_bed = load_img('environment/bunk_bed.png')
luon_bed_enter = load_img('characters/bed_interaction/luon_bed_enter_01.png')
backpack_prop = load_img('props/luon_backpack.png')
meal_tray = load_img('remaining/P00/meal_tray.png')
long_sit = load_img('characters/long_sitting.png')
to_sit = load_img('characters/to_sitting.png')
dam_sit = load_img('characters/dam_sitting.png')
toothbrush_cup = load_img('remaining/P01/toothbrush_cup.png')
luon_idle = load_img('characters/luon_idle_0.png')
bus_sign = load_img('commute/bus_sign.png')
bus_33 = load_img('commute/bus_33.png')
teacher = load_img('year1/teacher.png')
study_0 = load_img('commute/study_0.png')
notebook = load_img('commute/notebook.png')
luon_laptop = load_img('characters/luon_sitting_laptop.png')
long_gaming = load_img('remaining/C01/long_gaming_shout.png')
luon_typing_1 = load_img('year1_campaign/luon_typing_1.png')
long_typing_1 = load_img('year1_campaign/long_typing_1.png')
dam_writing = load_img('year1/dam_writing.png')
luon_exam_worried = load_img('remaining/C01/luon_exam_worried.png')
grade_stamp_a = load_img('ui/grade_stamp_a.png')
light_switch = load_img('environment/light_switch_socket.png')
luon_sleeping = load_img('characters/bed_interaction/luon_bed_sleeping.png')
pot_0 = load_img('year1/pot_0.png')
pot_1 = load_img('year1/pot_1.png')
hotpot_bag = load_img('remaining/P04/hotpot_supply_bag.png')
dam_floor = load_img('year1/dam_floor.png')
long_floor = load_img('year1/long_floor.png')
to_floor = load_img('year1/to_floor.png')
luon_floor = load_img('year1/luon_floor.png')

frames = []
durations = []

def add_frame(canvas, duration_ms=1300):
    frames.append(canvas.convert("RGB"))
    durations.append(duration_ms)

# ==========================================
# SCENE 1: Day 0 — Nhận phòng & Ký túc xá (Mốc 1/12)
# ==========================================
print("Composing Scene 1...")
# 1.1 Quầy tiếp nhận
c = bg_reception.copy()
place_sprite(c, npc_manager, 220, 170, scale=0.95)
place_sprite(c, luon_backpack_idle, 560, 220, scale=1.0)
place_sprite(c, keys_prop, 430, 280, scale=0.8)
draw_hud(c, "NHẬN PHÒNG & KÝ TÚC XÁ 404", "MỐC 1/12", "DAY 0 • 14:20")
draw_caption_card(c, "THỦ TỤC", BADGE_EVENT,
                  "Nộp giấy báo trúng tuyển • Quản lý KTX trao chìa khóa phòng 404",
                  "Vị trí: Bàn tiếp nhận KTX Khu B • Bắt đầu hành trình đại học")
add_frame(c, 1400)

# 1.2 Hành lang tới phòng
c = bg_hallway.copy()
place_sprite(c, luon_backpack_walk, 420, 200, scale=0.95)
draw_hud(c, "LÊN PHÒNG 404 KTX", "MỐC 1/12", "DAY 0 • 14:45")
draw_caption_card(c, "HÀNH LANG", BADGE_EVENT,
                  "Lên tầng 4 KTX • Tìm phòng 404 và mở khóa bước vào",
                  "Môi trường ký túc xá đông đúc, các tân sinh viên đang dọn vào")
add_frame(c, 1200)

# 1.3 Phòng 404: Chọn giường
c = bg_room404.copy()
place_sprite(c, bunk_bed, 140, 10, scale=0.75)
place_sprite(c, luon_bed_enter, 280, 210, scale=0.7)
place_sprite(c, backpack_prop, 480, 340, scale=0.8)
draw_choice_box(c, "LỰA CHỌN CHỖ NẰM PHÒNG 404", "Giường trên (leo thang, yên tĩnh)", "Giường dưới (thuận tiện di chuyển, cất balo)", selected='B')
draw_hud(c, "CHỌN GIƯỜNG & ĐẶT BALO", "MỐC 1/12", "DAY 0 • 15:10")
draw_caption_card(c, "LỰA CHỌN", BADGE_CHOICE,
                  "[ĐÃ CHỌN GIƯỜNG DƯỚI] Đặt balo, dọn đồ đạc cá nhân",
                  "Tầng dưới: không phải leo thang, thuận tiện sinh hoạt hàng ngày")
add_frame(c, 1500)

# 1.4 Căn tin: Làm quen
c = bg_canteen.copy()
place_sprite(c, long_sit, 240, 220, scale=0.9)
place_sprite(c, to_sit, 380, 220, scale=0.9)
place_sprite(c, dam_sit, 520, 220, scale=0.9)
place_sprite(c, meal_tray, 360, 310, scale=0.45)
draw_hud(c, "BỮA CƠM CĂN TIN ĐẦU TIÊN", "MỐC 1/12", "DAY 0 • 18:30")
draw_caption_card(c, "GIAO LƯU", BADGE_SUCCESS,
                  "Ăn tối tại căn tin KTX • Làm quen các bạn cùng phòng Long, Tơ, Đảm",
                  "Ngân sách: 30k/suất cơm • Kết thúc Ngày 0 an toàn và hào hứng")
add_frame(c, 1400)

# ==========================================
# SCENE 2: Day 1 — Buổi học đầu tiên (Mốc 2/12)
# ==========================================
print("Composing Scene 2...")
# 2.1 WC & Chuẩn bị
c = bg_wc.copy()
place_sprite(c, toothbrush_cup, 520, 280, scale=0.8)
place_sprite(c, luon_idle, 380, 220, scale=0.9)
draw_hud(c, "BUỔI HỌC ĐẦU TIÊN (HK1)", "MỐC 2/12", "DAY 1 • 06:40")
draw_caption_card(c, "ROUTINE", BADGE_ROUTINE,
                  "06:40 Chuông báo thức reo • Vệ sinh cá nhân & chuẩn bị tài liệu",
                  "Soạn balo: Giáo trình Giải tích 1 (08:00) & Vật lý 1 (10:00)")
add_frame(c, 1200)

# 2.2 Trạm bus 33
c = bg_campus.copy()
place_sprite(c, bus_sign, 220, 200, scale=0.9)
place_sprite(c, bus_33, 400, 210, scale=0.7)
place_sprite(c, luon_backpack_idle, 280, 260, scale=0.85)
draw_hud(c, "COMMUTE BUS 33 SANG LINH TRUNG", "MỐC 2/12", "DAY 1 • 07:15")
draw_caption_card(c, "DI CHUYỂN", BADGE_EVENT,
                  "Đón tuyến Bus 33 từ KTX Khu B sang Cơ sở Linh Trung",
                  "Thời gian di chuyển: 25 phút • Vé xe bus sinh viên: 5k/lượt")
add_frame(c, 1300)

# 2.3 Giảng đường H6
c = bg_classroom.copy()
place_sprite(c, teacher, 160, 160, scale=0.8)
place_sprite(c, study_0, 420, 240, scale=0.8)
place_sprite(c, notebook, 480, 320, scale=0.6)
draw_hud(c, "GIẢNG ĐƯỜNG H6 — TIẾT HỌC ĐẦU", "MỐC 2/12", "DAY 1 • 08:00")
draw_caption_card(c, "ĐIỂM DANH", BADGE_SUCCESS,
                  "08:00 GT1 / 10:00 VL1: Điểm danh CÓ MẶT • Nghe giảng & ghi chép bài",
                  "Hoàn thành 2 tiết học mở màn; nắm quy chế thi và đề cương chi tiết")
add_frame(c, 1400)

# 2.4 Về phòng chọn hoạt động
c = bg_room404.copy()
place_sprite(c, luon_laptop, 400, 230, scale=0.9)
draw_hud(c, "VỀ PHÒNG — CHỌN HOẠT ĐỘNG [D1-FREE]", "MỐC 2/12", "DAY 1 • 17:30")
draw_choice_box(c, "HOẠT ĐỘNG BUỔI TỐI PHÒNG 404", "Mở laptop làm bài tập Giải tích 1 (Giảm gap)", "Nghỉ ngơi, lướt web / đi dạo KTX", selected='A')
draw_caption_card(c, "TỰ DO", BADGE_CHOICE,
                  "Chọn làm bài tập GT1 • Cập nhật Assignment: 1/5 hoàn thành",
                  "Lên giường ngủ lúc 22:30 -> Chuẩn bị cho Day 2")
add_frame(c, 1400)

# ==========================================
# SCENE 3: Day 2 — Rẽ nhánh: Chọn chơi hay học (Mốc 3/12)
# ==========================================
print("Composing Scene 3...")
# 3.1 Long rủ chơi trước giờ học
c = bg_room404.copy()
place_sprite(c, long_gaming, 220, 200, scale=0.85)
place_sprite(c, luon_laptop, 520, 220, scale=0.85)
draw_choice_box(c, "LONG RỦ: 'LÀM VÁN GAME RỒI ĐI HỌC LUỒN ƠI!'", "Đi học đúng giờ (08:00 Nhập môn Tin)", "Chơi 1 ván game (Nguy cơ trễ học, mất điểm danh)", selected='B')
draw_hud(c, "NGÃ RẼ: CHƠI HAY HỌC TRƯỚC GIỜ LỚP?", "MỐC 3/12", "DAY 2 • 07:10")
draw_caption_card(c, "PHÂN NHÁNH", BADGE_CHOICE,
                  "[NHÁNH B] Cuốn vào trận game với Long • Trade-off thời gian thực",
                  "Cảnh báo hệ thống: Lớp NMT bắt đầu lúc 08:00 (T-50 phút)")
add_frame(c, 1500)

# 3.2 Cuốn vào game & Vắng học
c = bg_room404.copy()
place_sprite(c, long_typing_1, 260, 220, scale=0.9)
place_sprite(c, luon_typing_1, 520, 220, scale=0.9)
draw_hud(c, "COMBAT CĂNG THẲNG — QUÁ GIỜ ĐẾN LỚP!", "MỐC 3/12", "DAY 2 • 08:25")
draw_caption_card(c, "HẬU QUẢ", BADGE_ALERT,
                  "CẢNH BÁO T-0: Quá giờ vào lớp NMT • Hệ thống ghi nhận: VẮNG TIẾT 1!",
                  "Hậu quả: Attendance -1 • Kiến thức buổi 1 chưa được tiếp thu, phải tự học bù")
add_frame(c, 1500)

# 3.3 Buổi tối học bù
c = bg_room404.copy()
place_sprite(c, luon_laptop, 420, 220, scale=0.9)
place_sprite(c, notebook, 340, 320, scale=0.7)
draw_choice_box(c, "BUỔI TỐI: LONG TIẾP TỤC RỦ LEO RANK", "Tự học bù kiến thức NMT đã mất (Gap -1)", "Chơi game thâu đêm cùng bạn phòng (Quan hệ +2, Gap +2)", selected='A')
draw_hud(c, "BUỔI TỐI 404 — QUYẾT ĐỊNH HỌC BÙ", "MỐC 3/12", "DAY 2 • 20:00")
draw_caption_card(c, "HỌC BÙ", BADGE_SUCCESS,
                  "Quyết định học sớm: Hoàn thành tự học bài NMT bị lỡ",
                  "Bảo toàn tiến độ học tập • Rút kinh nghiệm quản lý thời gian")
add_frame(c, 1400)

# ==========================================
# SCENE 4: Day 28 — Ôn giữa kỳ (Mốc 4/12)
# ==========================================
print("Composing Scene 4...")
c = bg_room404.copy()
place_sprite(c, luon_laptop, 400, 210, scale=0.9)
place_sprite(c, notebook, 320, 310, scale=0.7)

overlay = Image.new("RGBA", (W, H), (0, 0, 0, 0))
draw = ImageDraw.Draw(overlay)
db_x0, db_y0, db_x1, db_y1 = 80, 110, 520, 250
draw.rounded_rectangle([db_x0, db_y0, db_x1, db_y1], radius=8, fill=(15, 20, 35, 240), outline=(130, 80, 230, 230), width=2)
draw.text((db_x0 + 15, db_y0 + 10), "DASHBOARD ÔN THI GIỮA KỲ 1", font=font_body, fill=(215, 175, 255))
draw.text((db_x0 + 15, db_y0 + 40), "• Giải tích 1:   [████████░░] 80% (Giới hạn, Đạo hàm)", font=font_small, fill=(220, 235, 255))
draw.text((db_x0 + 15, db_y0 + 65), "• Vật lý 1:      [███████░░░] 70% (Động học, Năng lượng)", font=font_small, fill=(220, 235, 255))
draw.text((db_x0 + 15, db_y0 + 90), "• N.môn Tin:     [██████████] 100% (C++ & Con trỏ)", font=font_small, fill=(120, 255, 180))
c.alpha_composite(overlay)

draw_hud(c, "ÔN TẬP GIỮA KỲ — TỔNG HỢP TOPICS", "MỐC 4/12", "DAY 28 • 19:30")
draw_caption_card(c, "DASHBOARD", BADGE_DASHBOARD,
                  "Mở dashboard môn học • Luyện đề thi thử & trau dồi các topic còn hổng",
                  "Sẵn sàng tâm lý cho đợt thi giữa kỳ diễn ra trong 2 ngày tới")
add_frame(c, 1500)

# ==========================================
# SCENE 5: Day 29 — Lớp học trước giờ G (Mốc 5/12)
# ==========================================
print("Composing Scene 5...")
c = bg_classroom.copy()
place_sprite(c, teacher, 180, 160, scale=0.8)
place_sprite(c, study_0, 480, 240, scale=0.8)
draw_hud(c, "GIẢNG ĐƯỜNG H6 — DẶN DÒ QUY CHẾ THI", "MỐC 5/12", "DAY 29 • 08:30")
draw_caption_card(c, "LỚP HỌC", BADGE_EVENT,
                  "Tiết ôn tập VL1 / NMT cuối cùng • Giảng viên dặn dò giới hạn đề thi",
                  "Phổ biến quy chế phòng thi: Không sử dụng tài liệu, mang thẻ sinh viên")
add_frame(c, 1300)

# ==========================================
# SCENE 6: Day 30 — Thi giữa kỳ 1 (Mốc 6/12)
# ==========================================
print("Composing Scene 6...")
c = bg_classroom.copy()
place_sprite(c, luon_exam_worried, 380, 190, scale=0.85)
place_sprite(c, grade_stamp_a, 620, 150, scale=0.45)
draw_hud(c, "KỲ THI GIỮA KỲ 1 — H6 CLASSROOM", "MỐC 6/12", "DAY 30 • 09:00")
draw_caption_card(c, "KẾT QUẢ", BADGE_SUCCESS,
                  "Hoàn thành bài thi Giải tích 1 & Vật lý 1 • ĐẠT ĐIỂM GIỎI (GRADE A)",
                  "Kết quả ghi nhận: Đạt tất cả mục tiêu topics • Tự tin hướng tới cuối kỳ")
add_frame(c, 1500)

# ==========================================
# SCENE 7: Day 92 — Chuẩn bị cuối kỳ 1 (Mốc 7/12)
# ==========================================
print("Composing Scene 7...")
c = bg_room404.copy()
place_sprite(c, luon_typing_1, 420, 210, scale=0.9)

overlay = Image.new("RGBA", (W, H), (0, 0, 0, 0))
draw = ImageDraw.Draw(overlay)
db_x0, db_y0, db_x1, db_y1 = 60, 110, 480, 230
draw.rounded_rectangle([db_x0, db_y0, db_x1, db_y1], radius=8, fill=(15, 20, 35, 240), outline=(220, 120, 40, 230), width=2)
draw.text((db_x0 + 15, db_y0 + 10), "NƯỚC RÚT CUỐI KỲ 1 (DEADLINE T-7)", font=font_body, fill=(255, 180, 80))
draw.text((db_x0 + 15, db_y0 + 40), "• Đồ án NMT: Nộp trước 23:59 [ĐÃ NỘP ✓]", font=font_small, fill=(120, 255, 180))
draw.text((db_x0 + 15, db_y0 + 65), "• Ôn thi GT1 & VL1: 12 chương trọng tâm", font=font_small, fill=(220, 235, 255))
c.alpha_composite(overlay)

draw_hud(c, "CHUẨN BỊ THI CUỐI KỲ 1 — NƯỚC RÚT", "MỐC 7/12", "DAY 92 • 21:00")
draw_caption_card(c, "DEADLINE", BADGE_CHOICE,
                  "Hoàn tất đồ án môn học • Luyện đề thi cuối kỳ xuyên màn đêm",
                  "Cân bằng thời gian: Tránh kiệt sức trước ngày thi chính thức")
add_frame(c, 1400)

# ==========================================
# SCENE 8: Day 99 — Thi cuối kỳ HK1 (Mốc 8/12)
# ==========================================
print("Composing Scene 8...")
c = bg_classroom.copy()
place_sprite(c, dam_writing, 340, 210, scale=0.85)
place_sprite(c, grade_stamp_a, 580, 130, scale=0.5)
draw_hud(c, "THI CUỐI KỲ HK1 — PHÒNG THI CHÍNH THỨC", "MỐC 8/12", "DAY 99 • 08:30")
draw_caption_card(c, "TỔNG KẾT HK1", BADGE_SUCCESS,
                  "KẾT QUẢ HK1: Giải tích 1: A | Vật lý 1: B+ | Nhập môn Tin: A",
                  "ĐÁNH GIÁ: XUẤT SẮC • Đủ điều kiện đăng ký toàn bộ môn học Kỳ 2")
add_frame(c, 1600)

# ==========================================
# SCENE 9: Day 113 — Bắt đầu học kỳ 2 (Mốc 9/12)
# ==========================================
print("Composing Scene 9...")
c = bg_campus.copy()
place_sprite(c, bus_sign, 200, 200, scale=0.9)
place_sprite(c, bus_33, 480, 210, scale=0.7)
place_sprite(c, luon_backpack_walk, 290, 240, scale=0.85)
draw_choice_box(c, "TÌNH HUỐNG LỠ CHUYẾN BUS 33 ĐẦU TIÊN", "Chạy nước rút đuổi theo xe bus (Tốn thể lực)", "Đợi chuyến 15 phút sau (Nguy cơ vào muộn tiết đầu)", selected='A')
draw_hud(c, "BẮT ĐẦU HỌC KỲ 2 — NHÁNH LỠ BUS", "MỐC 9/12", "DAY 113 • 07:05")
draw_caption_card(c, "TÌNH HUỐNG", BADGE_ALERT,
                  "Học kỳ 2 bắt đầu • Đồng hồ đếm ngược: Chạy nước rút kịp bắt Bus 33!",
                  "Lịch học mới: Giải tích 2, Vật lý 2, Cấu trúc Dữ liệu & Giải thuật")
add_frame(c, 1500)

# ==========================================
# SCENE 10: Day 155 — Sinh hoạt KTX (Mốc 10/12)
# ==========================================
print("Composing Scene 10...")
c = bg_room404.copy()
place_sprite(c, light_switch, 820, 200, scale=0.6)
place_sprite(c, long_sit, 240, 220, scale=0.85)
place_sprite(c, luon_sleeping, 520, 240, scale=0.75)
draw_hud(c, "SINH HOẠT KÝ TÚC XÁ PHÒNG 404", "MỐC 10/12", "DAY 155 • 23:00")
draw_caption_card(c, "SINH HOẠT", BADGE_ROUTINE,
                  "23:00 Thảo luận bài tập nhóm cùng Long • Tắt công tắc đèn ngủ",
                  "Luồn lên giường ngủ say • Nạp đầy năng lượng cho chuỗi ngày đồ án")
add_frame(c, 1400)

# ==========================================
# SCENE 11: Day 204 — Chuẩn bị cuối kỳ 2 (Mốc 11/12)
# ==========================================
print("Composing Scene 11...")
c = bg_room404.copy()
place_sprite(c, luon_typing_1, 400, 210, scale=0.9)
place_sprite(c, notebook, 320, 310, scale=0.7)

overlay = Image.new("RGBA", (W, H), (0, 0, 0, 0))
draw = ImageDraw.Draw(overlay)
db_x0, db_y0, db_x1, db_y1 = 60, 110, 520, 230
draw.rounded_rectangle([db_x0, db_y0, db_x1, db_y1], radius=8, fill=(15, 20, 35, 240), outline=(40, 170, 230, 230), width=2)
draw.text((db_x0 + 15, db_y0 + 10), "ÔN TẬP CUỐI KỲ 2 — BẢO VỆ GPA", font=font_body, fill=(120, 230, 255))
draw.text((db_x0 + 15, db_y0 + 40), "• CTDL & Giải thuật:  Cây nhị phân, Đồ thị [HOÀN TẤT ✓]", font=font_small, fill=(120, 255, 180))
draw.text((db_x0 + 15, db_y0 + 65), "• Giải tích 2:        Tích phân bội, Vi phân [85%]", font=font_small, fill=(220, 235, 255))
c.alpha_composite(overlay)

draw_hud(c, "ÔN TẬP CUỐI KỲ 2 — BẢO VỆ GPA", "MỐC 11/12", "DAY 204 • 20:30")
draw_caption_card(c, "MỤC TIÊU", BADGE_CHOICE,
                  "Tối ưu code giải thuật & làm đề thi mẫu • Sẵn sàng bảo vệ điểm số GPA cao",
                  "Quyết tâm hoàn thành trọn vẹn Năm 1 đại học không rớt môn nào")
add_frame(c, 1500)

# ==========================================
# SCENE 12: Day 211 — Thi xong & Ăn lẩu (Mốc 12/12)
# ==========================================
print("Composing Scene 12...")
# 12.1 Kết quả thi cuối năm
c = bg_classroom.copy()
place_sprite(c, luon_idle, 380, 220, scale=0.9)
place_sprite(c, grade_stamp_a, 560, 140, scale=0.55)
draw_hud(c, "KẾT QUẢ THI HỌC KỲ 2", "MỐC 12/12", "DAY 211 • 15:00")
draw_caption_card(c, "TỔNG KẾT NĂM 1", BADGE_SUCCESS,
                  "HOÀN THÀNH TOÀN DIỆN NĂM 1 • TẤT CẢ CÁC MÔN ĐỀU ĐẠT LOẠI GIỎI!",
                  "Thoát hiểm an toàn • Giữ vững học bổng và tư cách lưu trú KTX 404")
add_frame(c, 1500)

# 12.2 Nồi lẩu phòng 404 (Frame A)
c = bg_room404.copy()
place_sprite(c, hotpot_bag, 120, 240, scale=0.55)
place_sprite(c, meal_tray, 660, 280, scale=0.5)
place_sprite(c, pot_0, 390, 230, scale=0.55)
place_sprite(c, long_floor, 220, 260, scale=0.75)
place_sprite(c, to_floor, 330, 290, scale=0.75)
place_sprite(c, dam_floor, 520, 280, scale=0.75)
place_sprite(c, luon_floor, 620, 260, scale=0.75)
draw_hud(c, "ĂN MỪNG HOÀN THÀNH NĂM 1 — NỒI LẨU 404", "MỐC 12/12", "DAY 211 • 19:30")
draw_caption_card(c, "TIỆC LẨU", BADGE_SUCCESS,
                  "NỒI LẨU PHÒNG 404! • Luồn, Long, Tơ, Đảm quây quần ăn mừng kết thúc năm",
                  "Hương vị ấm cúng của tình bạn ký túc xá sau một năm học đầy nỗ lực")
add_frame(c, 1300)

# 12.3 Nồi lẩu sôi (Frame B)
c = bg_room404.copy()
place_sprite(c, hotpot_bag, 120, 240, scale=0.55)
place_sprite(c, meal_tray, 660, 280, scale=0.5)
place_sprite(c, pot_1, 390, 230, scale=0.55)
place_sprite(c, long_floor, 220, 260, scale=0.75)
place_sprite(c, to_floor, 330, 290, scale=0.75)
place_sprite(c, dam_floor, 520, 280, scale=0.75)
place_sprite(c, luon_floor, 620, 260, scale=0.75)
draw_hud(c, "ĂN MỪNG HOÀN THÀNH NĂM 1 — NỒI LẨU 404", "MỐC 12/12", "DAY 211 • 20:15")
draw_caption_card(c, "TIỆC LẨU", BADGE_SUCCESS,
                  "Nồi lẩu sôi sùng sục • Cùng chia sẻ thịt bò, rau xanh và kỷ niệm năm đầu",
                  "Chuẩn bị bước sang Năm 2 với những đồ án thực tế và thử thách mới!")
add_frame(c, 1300)

# 12.4 Kết thúc & Ngủ ngon
c = bg_room404.copy()
place_sprite(c, luon_sleeping, 440, 230, scale=0.8)
place_sprite(c, light_switch, 820, 200, scale=0.6)
draw_hud(c, "KHÉP LẠI NĂM 1 — SẴN SÀNG CHO NĂM 2", "MỐC 12/12", "DAY 211 • 23:59")
draw_caption_card(c, "HOÀN TẤT NĂM 1", BADGE_EVENT,
                  "Tắt đèn đi ngủ • Chuyến hành trình Năm 1 khép lại trọn vẹn và tự hào",
                  "Save game hoàn tất • SẴN SÀNG CHUYỂN TIẾP SANG CHIẾN DỊCH NĂM 2!")
add_frame(c, 2000)

print(f"Total frames: {len(frames)}")

# Save animated GIF
out_path_1 = os.path.join(OUTPUT_DIR, "T-001_year_one_flow.gif")
out_path_root = os.path.abspath(os.path.join(OUTPUT_DIR, "../../T-001_year_one_flow.gif"))

print(f"Saving to {out_path_1}...")
frames[0].save(
    out_path_1,
    save_all=True,
    append_images=frames[1:],
    duration=durations,
    loop=0,
    optimize=True
)

print(f"Saving copy to root {out_path_root}...")
frames[0].save(
    out_path_root,
    save_all=True,
    append_images=frames[1:],
    duration=durations,
    loop=0,
    optimize=True
)

file_size_mb_1 = os.path.getsize(out_path_1) / (1024 * 1024)
print(f"Done! File size: {file_size_mb_1:.2f} MB")
