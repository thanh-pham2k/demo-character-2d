# M04–M05: dọn phòng và bữa cơm đầu tiên

Scene: `mobile-2d/Assets/Scenes/Production_Opening.unity`.
Phạm vi hiện tại: prototype M01–M05, 17 sự kiện thành công. M06–M45 chưa được nối đủ vào scene này.

## M04

- Từ phòng xuống quầy căn tin, mua xô/giẻ 5.000đ, mang về phòng. Đi tới quầy chưa trừ tiền; mua thành công mới ghi nhận.
- Bắt đầu timer 60 giây; đi tới bốn điểm bụi, E/Tương tác để lau từng điểm trong 0,8 giây. Rag được hiển thị trong thao tác.
- Timeout chỉ khởi động lại phần dọn. Không mua lại dụng cụ, không trừ tiền lần nữa.
- Background pause và menu pause dừng timer. Tải lại mốc lưu trở về đầu phần dọn, giữ dụng cụ đã mua; các điểm đã lau dở là trạng thái tạm.
- Chỉ đủ bốn điểm mới ghi sự kiện dọn xong. Long, Tộ, Đạm xuất hiện qua ba đoạn làm quen; Đạm nói rõ học HCMUS.

## M05

- Nhóm rủ xuống căn tin; đi tới quầy chọn cơm và ít nhất một món ăn, trà tuỳ chọn.
- Cơm 5.000đ, thịt 10.000đ, rau 5.000đ, trà 3.000đ. Ba suất chính có giá 10.000/15.000/20.000đ; thêm trà thành 13.000/18.000/23.000đ.
- Thêm thịt làm tổng vượt ví sẽ bị chặn. Giá trên UI phải khớp lựa chọn trong dữ liệu trước khi commit; không trừ tiền khi chọn hoặc hủy menu.
- Thanh toán một lần, lưu lựa chọn món. Tải lại sau thanh toán về căn tin ở bước ăn, không charge lại.
- Bốn pose ngồi ghế C02 được tái sử dụng. Chỉ đổi phần hiển thị của Lươn trong đoạn ăn; khi kết thúc/cancel trả lại renderer đã chụp trước đó.
- Xem lịch học và tin hỗ trợ 100.000đ đầu kỳ; hỗ trợ chỉ ghi một lần. Sau đó cả nhóm về phòng ngủ, mở mục tiêu chuẩn bị balo M06, tín chỉ vẫn 0.

## Dữ liệu, save và asset

Menu mở rộng event `choose_meal` thành sáu lựa chọn. Định nghĩa cũ được lưu theo SHA256 trong `story/plans/production-definitions/`. Chỉ hash được liệt kê rõ trong `PRODUCTION_SAVE_COMPATIBILITY.json`, với lịch sử chưa vượt M04, được replay sang dữ liệu mới. Không tự chấp nhận hash lạ; không ghi đè primary cũ không tương thích khi tải. Backup prefix hợp lệ vẫn có thể phục hồi.

Không gọi imagegen. Dùng các sheet C00/C02/P00/P03/E01 đã có và asset phòng/rác cũ. Ba ảnh standing Long/Tộ/Đạm được căn khung từ ảnh cũ; tổng bản dẫn xuất hiện là 12, không cộng vào 178 output generate chính. Manifest và script dẫn xuất giữ nguồn/hash/scale.

Foreground quầy/bàn được crop native từ background căn tin. Metadata crop nằm tại `assets/mvp_sprites/remaining/review/canteen-staging-crops.json`. Đã sửa vị trí điểm lau/spawn về hành lang giữa phòng, thu hẹp lớp quầy để không cắt mặt Lươn và căn tâm/scale/rotation khay cơm trên bàn.

## Bằng chứng

- `review/opening-playmode/report.json`: PASS 271 kiểm tra, 0 lỗi, hai lựa chọn giường, toàn tuyến M01–M05.
- Bao gồm timeout (rút ngắn limit trong test rồi trả về 60 giây), background/menu pause, reload giữa phần dọn, nhận bạn lần lượt, sáu giá menu, chặn vượt ví, thanh toán/hỗ trợ một lần, resume căn tin và về phòng M06.
- `review/production-campaign-model.json`: PASS 346.785 assertion trên 192 tổ hợp lựa chọn của mô hình 45 mission; có kiểm tra tương thích prefix cũ. Đây là kiểm tra model, không chứng minh 45 mission đã chơi được.
- Ảnh `cleaning-partial.png`, `meal-menu.png`, `dinner-world.png`, `morning-ready.png` cùng thư mục Play Mode đã được duyệt sau chỉnh bố cục.

Còn lại: nối M06–M08 bằng hệ thống đi học hiện có, sửa fallback M07 thành chuyến xe sau; tiếp tục M09–M45, rồi audio, motion QA đầy đủ, camera/collider chi tiết, portrait/safe area và thử touch trên thiết bị. Không đánh hoàn tất goal từ kết quả prototype này.
