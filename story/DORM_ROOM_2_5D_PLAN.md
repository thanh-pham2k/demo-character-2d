# Kế hoạch đã chốt — prototype ký túc xá 2.5D

## Phạm vi và ảnh chuẩn

- Ảnh chuẩn: `mobile-2d/Assets/dorm_room/reference/room_reference.png`, 1536 × 1024, tỷ lệ 3:2. Đây là nguồn đối chiếu hình ảnh; giữ nguyên ảnh gốc.
- Kiểm tra nhanh: render 768 × 512, cùng tỷ lệ 3:2. Độ phân giải này không phải hệ tọa độ thiết kế; dùng tọa độ thế giới Unity cho bố cục.
- Duyệt hình ảnh cuối: render 1536 × 1024 và so sánh với ảnh chuẩn.
- Mục tiêu đầu tiên là prototype 2.5D với camera cố định và furniture có thể tái sử dụng. Không làm toàn bộ asset đẹp trước khi kiểm tra blockout.

## Phase 1 — Structure

1. Dựng blockout phòng với sàn, tường, cửa sổ và cửa.
2. Calibrate camera theo các mốc trong ảnh chuẩn, sau đó khóa position, rotation, projection, FOV hoặc orthographic size và aspect 3:2. Chỉ mở khóa nếu có bằng chứng projection ban đầu sai.
3. Dựng blockout giường, bàn, ghế; tạo `Bed.prefab`, `Desk.prefab`, `Chair.prefab` và nhóm chúng trong `BedSet_Blockout.prefab` ngay khi một bên hợp lý.
4. Đặt `LeftSet` và `RightSet` dùng chung prefab, transform độc lập; chỉ mirror khi hướng thang và chi tiết phù hợp. Không mặc định hai bên đối xứng tuyệt đối.
5. Checkpoint structure: tỷ lệ phòng, vị trí furniture, khoảng đi lại, khoảng mở cửa và các mốc so với reference. Chốt ngưỡng đo được khi có blockout; không bịa số đo từ ảnh.

## Phase 2 — Gameplay

1. Thêm player placeholder để kiểm tra tỷ lệ và khoảng trống.
2. Thêm semantic anchors khi có thiết kế tương tác: `ChairPoint`, `StudyPoint`, `ClimbPoint`, `SleepPoint`, `ExitPoint`.
3. Thiết lập và kiểm tra collision, occlusion và các tương tác walk, sit, study, climb, sleep, open door.
4. Checkpoint gameplay: tới được anchor, không collision sai, lớp che đúng, đủ chỗ di chuyển và pose dễ đọc từ camera đã khóa. Kiểm tra các pose/animation thực tế; anchor không tự bảo đảm tương tác đúng.

Player placeholder chỉ chứng minh tỷ lệ và khoảng trống; không coi là gameplay PASS. Người dùng sẽ cung cấp scene hoặc nhân vật gameplay mới để kiểm tra thực tế. Với gameplay mới, cung cấp checkpoint F8 theo cơ chế state/restore hiện có nếu cơ chế đó có trong scene mới; không tự giả định implementation.

## Phase 3 — Visual

Sau khi structure và gameplay đạt, thay blockout bằng geometry hoàn chỉnh, thêm materials/textures, chỉnh style và lighting. Render 1536 × 1024, đối chiếu reference, xác nhận tiêu chí visual trước khi kết thúc. Không dùng chỉ số sai lệch màu toàn ảnh làm bằng chứng duy nhất cho geometry, occlusion hay gameplay.

## Cách làm và giới hạn

- Tách geometry và material; ưu tiên một model tái sử dụng, với prefab variant khi cần khác biệt thực tế.
- Metadata số phải được đo hoặc suy ra từ asset và camera thực tế; không coi tọa độ minh họa là dữ liệu production. Chỉ xây hệ thống dựng scene tự động từ JSON khi quy tắc bố trí đã ổn định và có nhu cầu.
- Kiểm tra nguyên nhân trước khi chỉnh transform, camera, geometry, material hay sorting; regenerate không phải bước sửa mặc định.
- Theo `AGENTS.md`, không tự chạy test suite, lint, compile, Unity Play Mode, batch mode hoặc build trừ khi người dùng yêu cầu. Người dùng tự kiểm tra gameplay qua checkpoint. Screenshot/so sánh thực hiện khi được yêu cầu hoặc cho phép theo chỉ dẫn hiện hành.
- Tại thời điểm lưu: đây là kế hoạch đã chốt, chưa phải xác nhận prototype hoặc gameplay đã hoàn thành.
