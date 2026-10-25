# Năm 1 — bàn giao campaign sáu ngày

> **Phạm vi:** tài liệu handover của flow Năm 1 trong `Room404_MVP`. Đây không phải thứ tự production hiện tại. Xem [task tracker](tasks/README.md) và các nguồn chuẩn trong `story/README.md` để biết việc production kế tiếp.

Cập nhật 25/10/2026. Tài liệu này thay thế flow cũ “một buổi học → 32 tín chỉ → lẩu”.
Scene sử dụng: `mobile-2d/Assets/Scenes/Room404_MVP.unity`.

## Timeline thực tế

| Mốc | Bắt buộc trước khi đi tiếp |
|---|---|
| Nhận phòng | Xí giường, dọn ba chỗ rác, gặp bạn cùng phòng, lấy balo, ra hành lang xuống căn tin |
| Bữa cơm đầu | Chọn cơm thịt kho/rau hoặc trứng/đậu; 25.000đ; nhận 100.000đ sinh hoạt đúng một lần |
| Ngày 1, HK1 | Giải Tích 1 buổi 1, Vật Lý 1 buổi 1; xe về KTX, lên phòng làm bài tập, ngủ |
| Ngày 2, HK1 | Nhập môn Tin buổi 1, Giải Tích 1 buổi 2; bài tập; chơi cùng Long, tranh cãi với Đạm, tự đứng dậy, tắt đèn, ngủ |
| Ngày 3, HK1 | Vật Lý 1 buổi 2, Nhập môn Tin buổi 2; bài tập, ôn và kiểm tra ba môn; ngủ; thẻ tổng kết 16/150 |
| Ngày 4, HK2 | WC, thử thách đón xe; Giải Tích 2 buổi 1, Vật Lý 2 buổi 1; bài tập, ngủ |
| Ngày 5, HK2 | Lập trình buổi 1, Giải Tích 2 buổi 2; bài tập, ôn khi đủ điều kiện, ngủ |
| Ngày 6, HK2 | Vật Lý 2 buổi 2, Lập trình buổi 2; bài tập, ôn và kiểm tra ba môn; ngủ; thẻ 32/150, không nợ môn |
| Kết năm | Tự tới điểm chuẩn bị lẩu trong phòng, ăn lẩu và thoại tự mãn; tự chuyển Năm 2, 23:40, lời rủ của Long |

Mỗi tiết kết thúc ở lớp; phải ra H6 và tới trạm xe về, lên KTX và về phòng.
Giữa ngày có thẻ “vài tuần sau”. Lớp Toán H6-101, Vật Lý H6-102 và phòng máy
H6-201 dùng hai backdrop được chuẩn hóa chibi, giữ kiến trúc H6 tham khảo.
Các môn có câu hỏi dễ riêng cho bài học, bài tập, ôn và kiểm tra; sai/hết giờ có gợi ý và thử lại không giới hạn.
Tích hợp asset song song hiện dùng bạn cùng lớp C00 tại H6; Đạm ở KTX và có đoạn nghe Lươn kể chuyện học sau chuyến về. Cờ story:h6-told-dam chống lặp đoạn này khi load.
32 tín chỉ là quy đổi chương trình qua sáu môn đại diện; UI giải thích điều này.

## Điều khiển và UI

- Di chuyển như trước; E dùng generic nearest IInteractable, Space tiếp thoại.
- Ghế: E ngồi; Game / Học tập / Đứng dậy. Học tập có sáu môn và Bài tập / Ôn thi / Kiểm tra; chưa đến mốc hiển thị lý do khóa.
- Bài giấy dùng pose viết; Tin/Lập trình dùng pose bàn phím. Sáu frame gõ/nghỉ/rê chuột, hai người lệch nhịp và có âm thanh theo nhịp.
- Ngày 2 phải E đứng, đi tới công tắc, E tắt, đi giường E; không tự bỏ qua thao tác.
- Hai người dùng hai ghế/laptop và hai giường riêng. Có vào giường, ngủ và thức dậy; hủy hoạt động trả chỗ và movement.
- Esc khi đang đứng ở trạng thái an toàn mở Tiếp tục / Chơi lại. Không mở giữa cutscene, ngủ, puzzle hoặc modal học tập.

## Xe, thời gian, tiền

Outbound tiến từ trái sang phải, homebound từ phải sang trái; entry/park/exit
riêng. Mỗi lượt reset trạng thái, vé 7.000đ trừ một lần khi lượt hoàn tất.
Bắt đầu 35.000đ; sau cơm và trợ cấp còn 110.000đ; đủ 12 vé còn 26.000đ.
Ngày 4 có 120 giây điều khiển, đồng hồ 07:20–07:40; menu/loading/interaction
khóa điều khiển dừng đồng hồ. Tới trạm kịp thì giữ suất, không tính animation
xe vào là trễ. Trễ có lựa chọn đi bộ miễn phí; năm học vẫn hoàn thành được.
Không thu thêm tiền ăn mỗi ngày, không thêm đói/kiếm tiền/nợ môn Năm 1.

## Kiến trúc

`YearOneSchedule` dữ liệu môn/lịch; `YearOneProgress` pure serializable state và
ID idempotent; `YearOneCampaignController` mutation/checkpoint; partial
`YearOneCampaignFlow` trong `YearOneMilestoneController` trình diễn/sự kiện.
`MVPFlowController` chỉ phát sự kiện mở đầu, giữ compatibility và nhận Year2.
Detector không có nhánh theo tên chair/bed/door. `PropReservation` dùng chung
cho player và Long. `RoommateActivityController` đi theo marker, không NavMesh.

Lưu `year-one-v1.json` trong `Application.persistentDataPath`, version 1, ghi
`.tmp` rồi thay thế file và giữ `.bak`. Load chính hỏng sẽ thử backup. Chỉ lưu
mốc an toàn, không lưu coroutine, pose hoặc UI. Continue dựng lại actor đứng
ở Room/Campus/Gate; chuyến chưa xong trở lại checkpoint trước, không mất vé.
Trên Mac hiện tại: `~/Library/Application Support/DefaultCompany/mobile-2d/`.
Bộ test dùng thư mục tạm riêng, không sửa save của người dùng.

## Files và scene integration

Files mới chính trong `Assets/Scripts/`:
`YearOneSchedule`, `YearOneProgress`, `YearOneCheckpointStore`,
`YearOneCampaignController`, `YearOneCampaignFlow`, `YearOneCampaignUI`,
`PropReservation`, `RoommateActivityController`, `DeskPoseTiming`,
`ClassroomNpcAnimation`.

Files hiện có được mở rộng:
`YearOneMilestoneController`, `YearOneInteractable`, `MVPFlowController`,
`CampusCommuteController`, `BusStopInteractable`, `DormJourneyController`,
`BackpackInteractable`, `ChairInteractable`, `BedInteractable`,
`LuonCharacterVisual`, `DeskActionModal`, `CalculusPuzzleModal`.

Editor: `YearOneCampaignSceneSetup`, `YearOneCampaignPlayChecks`,
`YearOneProgressChecks`, `YearOneMotionCapture`, `YearOnePosePreview`;
setup cũ và suite cũ được nối vào campaign mới. PosePreview là fixture tùy
chọn, không phải chứng minh acceptance hoặc shortcut trong sản phẩm.

Scene đã serialize controller/UI/course buttons, hai laptop, Long activity và
pose registration, canteen/party markers, class exit, return bus markers,
feast preparation và công tắc có vùng E chạm tới được. Reuse SeatPivot/
BedPivot/ExitPivot, foreground và actor hiện có. Không cần assign Inspector
thủ công; không còn thiếu asset bắt buộc của scope này.

## Asset provenance và kiểm chứng

37 sprite PNG runtime trong `Assets/Sprites/MVP/year1_campaign/`; năm source
batch và prompt/crop/anchor/script trong `assets/mvp_sprites/year1_campaign/`.
Batch sửa Long bed thay thế chín pose góc nhìn ngang; các crop cũ còn trong
manifest để truy vết, không dùng trong scene. Xem `GENERATION.md`.

Báo cáo nghiệm thu cuối: `assets/reviews/year1_campaign/REVIEW.md`.
JSON và ảnh test: `mobile-2d/PlayModeInteractionReports/`.
Video Play Mode thực tế: `assets/reviews/year1_campaign/*.mp4`.
Video không có track âm thanh; audio được kiểm tra runtime riêng.

Kết quả cuối: **506 PASS / 0 lỗi** (UTC 2026-10-25T10:55:36.0886730Z). Xem REVIEW.md, report.json và video. Các báo cáo
160/164 check thuộc flow cũ và các lần 480/493 là bước kiểm tra trước bản cuối.

Sau full regression 506: marker công tắc được đặt dưới ổ điện cùng trục X; kiểm tra Play Mode bổ sung movement/detector/Interact PASS. Chi tiết và giới hạn nằm trong REVIEW.md.
