# Gameplay mở đầu M01–M03

Scene: `mobile-2d/Assets/Scenes/Production_Opening.unity`.
Đây là scene production riêng đang được triển khai theo baseline; chưa thay scene demo `Room404_MVP` và chưa nối đầy đủ M04–M45.

## Đã nối vào runtime

- M01: đi tới người mời xe, trò chuyện/từ chối; đi mua vé 7.000đ; đến cửa xe và chuyển tới sảnh KTX. Vé chỉ trừ một lần và được tiêu thụ khi đến nơi.
- M02: kéo giấy nhập học vào ô tiếp nhận. Kéo balo bị từ chối; không nhận khóa khi chưa nộp giấy. Nhận chìa khóa 404 rồi tới cầu thang bên trái sảnh, lên tầng 4.
- M03: dùng khóa vào phòng; chọn giường trái/phải; đặt balo đúng giường đã chọn; giữ nút hoặc Space trong 3 giây để siết ốc và cắm quạt.
- Thành công mới ghi sự kiện vào `ProductionCampaignSession`. Hủy, thả nút hoặc tạm dừng không hoàn thành thao tác. Đoạn chuyển cảnh giữ quyền điều khiển cho tới khi đến điểm an toàn.
- Esc mở menu tiếp tục checkpoint khi đang đứng; khi có modal, Esc hủy modal. Khóa di chuyển/physics được trả lại khi đóng UI. Save riêng nằm trong thư mục `production` thuộc persistentDataPath; các test dùng thư mục riêng.
- Kết thúc M03 mở mục tiêu M04, chưa đánh Finished. Các thao tác M04 trở đi vẫn cần được nối.

## Asset và trình bày

- Dùng C00 người mời xe/quản lý; E00 bến xe/sảnh; P00 quạt nhỏ; xe buýt, phòng, giường, bàn và balo có sẵn. Không gọi imagegen trong đợt này.
- Chín sprite ngang cũ được căn về canvas/feet thống nhất bằng script `assets/mvp_sprites/remaining/derived-existing/normalize_existing.py`. Manifest ghi từng ảnh nguồn/hash/scale. Đây là asset dẫn xuất từ ảnh cũ, không phải batch generate mới và không cộng vào 178 output chính.
- `LuonCharacterVisual` được cấu hình tùy chọn cho hướng front/back mới và ngang tái sử dụng; hỗ trợ sơ mi/áo tốt nghiệp. Vẫn một lớp điều khiển sprite, không gắn Animator thứ hai. Scene cũ không bật catalog giữ đường hiển thị cũ.
- UI, label, giấy tờ, tiến trình và văn bản tiếng Việt dựng bằng Unity. Đã chỉnh xe về mặt đường, giới hạn kích thước balo/quạt, nền HUD, lớp modal và thanh hold hiển thị đúng giá trị.

## Kiểm tra và giới hạn

Bằng chứng: `assets/mvp_sprites/remaining/review/opening-playmode/report.json` và các ảnh cùng thư mục.
Bộ Play Mode thực hiện cả hai lựa chọn giường bằng movement/input thật, kiểm tra từ chối tương tác từ xa, khóa/hủy modal, kéo sai/đúng giấy, vé một lần, chuyển vùng, pause/resume checkpoint, đặt balo, hold/release/background pause, hướng nhìn, chân/pivot và các bộ trang phục.

Ảnh chụp được duyệt riêng; số test qua không thay cho kiểm tra trình bày. Camera/đường đi hiện là bố cục kiểm tra mở đầu. Layout portrait/safe area, raycast touch trên thiết bị, audio, animation lên xe/mở cửa chi tiết và các collider đồ nội thất còn thuộc cổng hoàn thiện. Thư mục report phải được đối chiếu với source/scene hiện tại; không dùng kết quả cũ để xác nhận sửa đổi mới.

Tiếp theo: nối mua bộ vệ sinh, dọn bốn điểm, bạn đến lần lượt và căn tin M04–M05; sau đó nối campaign năm nhất và sửa fallback M07 thành chuyến xe sau theo baseline.

Kết quả lượt cuối: PASS 125 kiểm tra Play Mode, 0 lỗi; ảnh chụp đã duyệt sau chỉnh scale/UI/thanh hold. Hash scene/source/report lưu trong `opening-runtime-evidence.json`.

## Mở rộng hiện tại

Scene đã mở rộng tới hết M05; xem [PRODUCTION_HOUSEKEEPING_REVIEW.md](PRODUCTION_HOUSEKEEPING_REVIEW.md). Báo cáo hiện tại tại đường dẫn trên là lượt M01–M05 với 271 kiểm tra qua, thay cho lượt 125 kiểm tra mở đầu. Tổng sprite dẫn xuất từ ảnh cũ hiện là 12. M06–M45 và các cổng hoàn thiện vẫn đang thực hiện.
