# Năm 2 — sự kiện mẫu và kết nối v0.1

Các event và số liệu là fixture thiết kế, không phải bản triển khai. Calendar W02/W04/W06–W09 cần một ngày cụ thể; NPC availability là dữ liệu của fixture, không mặc định cho campaign. Áp dụng idempotency, priority và interruption trong `02_He_thong_thoi_gian_va_su_kien.md`.

## Fixture

Player vào Y2 với `YearSnapshot` đã commit. `wallet=30M`, E=75 chỉ khi fixture được tải độc lập; playthrough thật giữ state, không reset tiền/E. Player biết H6. Các event không phụ thuộc điểm quan hệ trừ khi ghi rõ; Long, Minh, An, Nam chỉ biết những việc họ chứng kiến hoặc được kể.

## EV_Y2_FAIR — Hội CLB

- Major, W02 `[09:00,12:00)`, LOC_CLUB; trigger khi tới hội và đã biết lịch, instance một lần. Mục tiêu: khám phá hoạt động; xung đột là fair tốn thời gian trong khi lịch học/bài có thể trùng.
- `explore`: 45 phút, E −5, 0 M; khi hoàn thành, player biết bốn lựa chọn coding, thể thao, nhạc, tình nguyện. `skip`: 0 phút, không tạo memory. `leave_during`: chỉ trả thời gian/E đã dùng, không tự cấp thông tin tất cả CLB.
- Khi explore xong player có thể `join` đúng một CLB tại fair hoặc `join_none`, 0 phút; không thể enroll lại ngay trong cùng instance. Đây là giới hạn cơ hội tại fair, không ngăn đăng ký khác nếu event về sau được author. Chọn coding mở lời mời phiên coding W03; các CLB khác cần record tương ứng, không hứa có event chưa được author.
- Event này không tự cho gặp An/Nam: nếu có mặt và player chọn nói chuyện mới tạo `met_an`/`met_nam`. Memory `club_fair` lưu lựa chọn và actors thực tham gia; trust chỉ thay đổi trong event hội thoại riêng. Academic/career: không tăng kỹ năng chỉ vì ghi danh; mở cơ hội hoạt động.
- Hết fair/không tới: event expired, không trừ quan hệ, tự học/project độc lập vẫn có thể mở. Save/reload sau join không tạo hai membership.

## EV_Y2_WORK_NOTICE — Ca làm thêm

- Medium, W03 từ 08:00 tới W04 16:00, LOC_CAFE hoặc bảng tin H6; không yêu cầu CLB. Player đọc thông tin 0 phút: ca W04 17:00–20:00, E −25, 40 M thanh toán sau khi hoàn thành, điều kiện đi lại tới CAFE và chính sách hủy. Đây là tiền game, không biểu thị mức lương thực.
- `apply`: 15 phút, E −2; xác nhận hợp đồng sau 0 phút nếu slot còn trống; `later`: 0 phút; không ứng tuyển không bị trừ điểm. `shift_reserved` không tự di chuyển player hoặc tăng tiền.
- Có thể hủy trước 17:00; fixture không phạt tiền hoặc quan hệ nếu đã hủy. Không tới lúc 17:00 thì `no_show`, manager biết; chưa author phản ứng nên không tự giảm trust. Nếu ca hoàn thành, ledger ghi +40 một lần, `work_shift_done=true`; E theo activity, không trả thu nhập nếu dừng sớm trong fixture. Chính sách lương một phần là quyết định cần xem lại với playtest.
- Follow-up: `EV_Y2_SHIFT_EXAM` chỉ nếu được mời thêm ca khác, không suy ra từ một ca đã làm.

## EV_Y2_GROUP_FORM — Chọn nhóm

- Major, W06 `[09:00,17:00)`, LOC_CLASS, enrolled trong course dự án nhóm `C_TEAM` fixture. Giảng viên cho phép team 2 hoặc cá nhân; chưa author course grade, event không cộng credit. `met_minh=true` và Minh available mới có lựa chọn mời Minh, không đảm bảo Minh chấp nhận nếu có lịch xung đột.
- `invite_minh` hoặc `invite_s04`: 15 phút, E −2, 0 M; nếu NPC chấp nhận, tạo project/team và ghi actor; nếu bận, thông báo và trả lại panel với thời gian đã dùng. Khi team với Minh được tạo, callback theo `60` chỉ dùng memory Minh biết, không tự đổi effect. `solo`: 0 phút, tạo project solo. `defer`: 0 phút; hết cửa sổ chưa chọn thì calendar đặt nhóm cá nhân theo quy tắc fixture đã thông báo.
- `Project`: 4 milestone effort, ban đầu 0; deadline W08 20:00. Một `A_GROUP_PROJECT`/`A_PERSONAL_PROJECT` 90 phút, E −15 tăng một milestone khi hoàn tất; partial giữ elapsed, không nhận milestone trước completion. Không tự cho NPC làm việc khi chưa có timeline được author.
- Cần rubric/môn, lịch team và route trước production. Conflict event chỉ mở với team, không tự kích hoạt cho solo.

## EV_Y2_TEAM_CONFLICT — Ai làm phần còn lại?

- Conditional major, W07, LOC_PROJECT; `project.team_size=2`, deadline chưa qua, cả hai available, `player_effort=1`, `partner_effort=0`, và outcome đối thoại chưa commit. Điều kiện effort trên chỉ là fixture; nếu không đúng thì không mở cảnh, không gán partner lười.
- `redistribute`: 30 phút E −5, partner đồng ý theo lịch fixture; đặt `agreed_scope` và lịch partner contribution W08 10:00. Khi tới mốc và partner available, tăng 1 milestone một lần; NPC có lịch bận thì thông báo và không tự cấp.
- `do_extra`: chạy `A_GROUP_PROJECT` 90 phút E −15 cho 1 milestone; mở conflict nếu player trong dialogue chọn bày tỏ bức xúc, không tự giảm trust. `ask_lecturer`: 30 phút E −5, Hạnh available mới nhận; mở `review_meeting`, không tự thay đổi grade. `leave`: 0 phút, giữ scope và deadline.
- Quan hệ: nếu player chọn cáo buộc trực tiếp trong lời thoại riêng, mở `team_conflict` và resentment +3; bàn việc hoặc nhờ giảng viên không tự phạt. Callback M08 chỉ dùng sự kiện Minh tham gia thực sự; S04 có variant riêng. `team_conflict` không tự đóng sau submission.
- Miss: player không gặp team trước deadline thì event expired; project vẫn có thể làm solo phần của mình nếu data course cho phép. Deadline ghi complete/partial theo milestone, không xóa artifact.

## EV_Y2_SHIFT_EXAM — Ca thêm trước bài thi

- Major, W09 17:00; player đã biết lớp thi sáng hôm sau; manager mời ca W09 `[18:00,21:00)` nếu player từng làm ca và manager available. E −25, +40 M khi hoàn tất; 0 phí. Tại thời điểm chọn, player có thể tự học 60 phút E −10 hoặc nghỉ 30 phút +15 E; UI cho thấy thời gian/energy và nhắc ngày thi, không dự đoán kết quả thi chắc chắn.
- `take_shift`: reserve ca, player phải tới CAFE trước 18:00 và hoàn thành để được trả tiền. `study`: khởi chạy A_STUDY, chọn topic đã mở. `rest`: A_REST. `not_now`: không trôi thời gian, invitation hết ở 18:00 nếu không nhận. Không có lựa chọn nào trực tiếp pass/fail môn.
- Nếu thiếu E để hoàn thành ca, UI cảnh báo theo rule energy và cho nghỉ trước giờ nếu còn đủ thời gian; dừng giữa ca xử lý theo hợp đồng fixture. Miss ca có record, không phát tiền. Thi vẫn dùng course record và đáp án; kiến thức/evidence chỉ ảnh hưởng hint theo assessment data cụ thể.
- Player không đi làm thêm: event không trigger; một sự kiện ôn/nghỉ thường vẫn có thể dùng nếu calendar author. Không tạo memory về ca nếu không tham gia.

## Các giới hạn cần xử lý

Dữ liệu timestamp, course project, lịch NPC, các CLB ngoài coding, project rubric, lương khi ngắt và event recovery chưa hoàn chỉnh. Cần QA tại boundary 12:00/18:00, insufficient energy, save trước/sau payout, NPC vắng, nhóm solo, thiếu calendar và conflict chưa đóng. Chưa chạy mô phỏng hoặc playtest, không kết luận cân bằng.
