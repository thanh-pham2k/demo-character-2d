# Địa điểm và hoạt động — catalogue đề xuất v0.1

Đây là dữ liệu thử thiết kế, không phải nội dung đã chạy hay cân bằng. ID dùng nhất quán trong tài liệu này; số phút/năng lượng/tiền là cấu hình giả lập. Không suy ra đã có calendar, NPC, event hay asset. Y = năm; `M` = đơn vị tiền game, chưa quy đổi tiền thật; E là energy. Thời gian travel là activity riêng; địa điểm ẩn cho đến khi biết hoặc được chỉ đường. Các event ID tại cột cơ hội là seed cần author.

## Địa điểm (15)

| ID | Địa điểm; mở | Cơ hội có ý nghĩa | Kết nối / giới hạn |
|---|---|---|---|
| LOC_ROOM | Phòng 404; Y1 arrival | A_STUDY, A_SLEEP, A_GAME_LONG | Hành lang; Long chỉ nếu rảnh |
| LOC_CORRIDOR | Hành lang KTX; Y1 | A_TALK_NEIGHBOR, A_CHECK_BOARD | 404, WC, thang; không tự mở mọi cửa |
| LOC_WC | WC; Y1 | A_HYGIENE, A_WASH_FACE | Hành lang; routine không bắt buộc lặp mỗi ngày |
| LOC_STAIRS | Cầu thang; Y1 | A_GO_DOWN, A_GO_UP | Hành lang/sảnh; di chuyển có cost |
| LOC_LOBBY | Sảnh; Y1 | A_REGISTER_DORM, A_REPORT_ISSUE | Thang, căn tin, trạm; S01 có ca |
| LOC_CANTEEN | Căn tin; Y1 | A_MEAL, A_MEAL_LONG, A_CAFE_SHIFT | Sảnh; ca làm từ Y2 theo lịch |
| LOC_BUS | Trạm; Y1 | A_RIDE_BUS, A_WAIT_BUS, A_TALK_STOP | Sảnh/H6; tuyến có giờ và giá |
| LOC_H6 | Khu H6; Y1 | A_FIND_CLASS, A_CHECK_H6_BOARD | Trạm, lớp, thư viện; không tự vào lớp |
| LOC_CLASS | Lớp; Y1 | A_ATTEND, A_ASK_MINH, A_GROUP_MEET | H6; lớp/giờ NPC theo calendar |
| LOC_LIBRARY | Thư viện; Y1 khi biết | A_LIBRARY_STUDY, A_GROUP_STUDY, A_RESEARCH | H6; giờ mở cửa cần author |
| LOC_CLUB | Phòng CLB; Y2 khi biết | A_CLUB_FAIR, A_CLUB_SESSION, A_CLUB_ORGANIZE | H6; tham gia không là điều kiện tốt nghiệp |
| LOC_CAFE | Quán gần trường; Y2 khi biết | A_CAFE_MEET, A_CAFE_SHIFT, A_CAFE_REST | Khu trường; tùy giờ và ngân sách |
| LOC_PROJECT | Không gian dự án; Y2 | A_GROUP_PROJECT, A_HACKATHON, A_PROJECT_REVIEW | H6/online; phụ thuộc lịch sự kiện |
| LOC_CAREER | Hội trường hướng nghiệp; Y3 | A_CAREER_FAIR, A_CV_REVIEW, A_MOCK_INTERVIEW | H6; event theo window |
| LOC_WORK | Nơi làm/online; Y2–6 hoặc sau tốt nghiệp | A_TUTOR, A_INTERNSHIP_SHIFT, A_INTERVIEW | Điểm đến theo hợp đồng; không mặc định có việc |

Các cơ hội tại cầu thang/WC chủ yếu là chức năng, chưa đạt chuẩn 2–3 cơ hội có quyết định tương đương các địa điểm chính; cần review thay vì thêm hoạt động giả chỉ để đạt chỉ tiêu.

## Quy ước bảng hoạt động

Điều kiện luôn gồm năm, địa điểm, cửa sổ mở; `NPC` nghĩa là đã gặp, đang available và đồng ý; `cal` nghĩa là entry author active; `funds` nghĩa là đủ tiền chưa reservation. Không có cost hoàn tiền mặc định; cost thu sau xác nhận, elapsed không trả lại nếu ngắt. `repeat` = lặp khi điều kiện thỏa; `once` = mỗi event/entry một lần; `day` = một lần/ngày; `shift` = một lần/ca; `window` = theo cửa sổ; event/cooldown chi tiết phải có trong data. E và M trong ngoặc là thay đổi (số âm là chi phí), `?` là tác động có điều kiện chứ không chắc chắn. Mọi activity phải preview chi phí thực tế và xung đột lịch; chưa có config thì không thể triển khai.

| ID · tên | Loại; Y; location | Điều kiện | Phút; ΔE; ΔM | Effect / skill / relation | Event / unlock; rủi ro; cooldown |
|---|---|---|---|---|---|
| A_STUDY · tự học | học; 1–6; ROOM | topic mở | 60; -10; 0 | +10 evidence topic; không đổi quan hệ | topic feedback; trễ deadline khác; repeat |
| A_SLEEP · ngủ | cá nhân; 1–6; ROOM | có giường | 480; +60; 0 | phục hồi E | preview skip; lỡ sự kiện; day |
| A_REST · nghỉ | cá nhân; 1–6; ROOM/CAFE | chỗ nghỉ | 30; +15; 0 | phục hồi E | đổi lịch; repeat |
| A_GAME_LONG · chơi cùng Long | xã hội; 1–6; ROOM | NPC | 30; -5; 0 | memory chơi; trust thay đổi theo event | phiên thông thường; có thể lỡ lịch; window |
| A_GAME_LONG_QUICK · một lượt nhanh | xã hội; 1; ROOM | NPC, invitation D2 | 15; -3; 0 | memory/trust theo `EV_Y1_LONG_INVITE` | L03; có thể lỡ xe; window |
| A_HYGIENE · đánh răng | cá nhân; 1–6; WC | WC mở | 5; 0; 0 | cờ vệ sinh theo ngày | phản hồi routine; trễ lịch; day |
| A_WASH_FACE · rửa mặt | cá nhân; 1–6; WC | WC mở | 5; 0; 0 | cờ tỉnh táo; chưa có hệ số học | routine; trễ lịch; day |
| A_REGISTER_DORM · nhận phòng | khám phá; 1; LOBBY | cal arrival | 20; -2; 0 | chìa phòng theo event | Y1_ARRIVAL; trễ giờ ăn; once |
| A_REPORT_ISSUE · báo sự cố | xã hội; 1–6; LOBBY | S01 available + issue | 15; -1; 0 | issue reported | lịch sửa chữa; chờ; once/issue |
| A_MEAL · ăn | cá nhân; 1–6; CANTEEN | funds | 30; +5; -8 | hồi E | gặp nhân vật có điều kiện; tốn tiền; repeat |
| A_MEAL_LONG · ăn với Long | quan hệ; 1–6; CANTEEN | NPC + funds | 45; +5; -8 | memory bữa ăn, relation theo event | L02; lỡ lịch; window |
| A_TALK_NEIGHBOR · hỏi phòng bên | khám phá; 1–6; CORRIDOR | NPC available | 15; -1; 0 | biết thông tin hoặc NPC | neighbor intro; chưa đảm bảo kết bạn; window |
| A_CHECK_BOARD · xem bảng KTX | khám phá; 1–6; CORRIDOR | bảng có thông báo | 5; 0; 0 | nhận thông tin event đã công bố | cơ hội KTX; có thể hết hạn; day |
| A_GO_DOWN · xuống | di chuyển; 1–6; STAIRS | lối mở | 3; -1; 0 | sang LOBBY | không; trễ lịch; repeat |
| A_GO_UP · lên | di chuyển; 1–6; STAIRS | lối mở | 3; -1; 0 | sang CORRIDOR | không; trễ lịch; repeat |
| A_RIDE_BUS · đi xe | di chuyển; 1–6; BUS | xe dừng + funds | 15; -2; -3 | chuyển H6/KTX | sự kiện xe; lỡ lớp; window |
| A_WAIT_BUS · chờ xe | di chuyển; 1–6; BUS | có chuyến được author | thời gian đến xe; 0; 0 | clock đến chuyến | xe đến; có thể lỡ lịch; repeat |
| A_TALK_STOP · trò chuyện tại trạm | xã hội; 1–6; BUS | NPC | 10; -1; 0 | thông tin tuyến hoặc memory | gặp bạn; lỡ xe nếu lâu; window |
| A_FIND_CLASS · tìm lớp | khám phá; 1–6; H6 | có địa chỉ lớp | 10; -2; 0 | biết LOC_CLASS | first class; vào muộn; once/lớp |
| A_CHECK_H6_BOARD · xem bảng | khám phá; 1–6; H6 | bảng hiện thông báo | 5; 0; 0 | biết event/schedule công khai | club fair/career fair; repeat/day |
| A_ATTEND · học trên lớp | học; 1–6; CLASS | enrolled + class active | thời gian còn lại; -10; 0 | attendance + evidence topic theo lớp | first class; late/absent; once/entry |
| A_ASK_MINH · hỏi Minh | học/quan hệ; 1–6; CLASS | NPC + topic mở | 20; -3; 0 | +5 evidence topic; relation theo event | M02; lỡ lịch; window |
| A_ASSIGNMENT · làm bài | học; 1–6; ROOM/LIBRARY | assignment mở | 90; -15; 0 | +1 milestone assignment | deadline; dở dang nếu ngắt; repeat |
| A_SUBMIT · nộp bài | học; 1–6; ROOM/CLASS | assignment có bản nộp | 5; 0; 0 | submit snapshot | feedback; overdue; once/version |
| A_LIBRARY_STUDY · học thư viện | học; 1–6; LIBRARY | topic mở | 60; -10; 0 | +10 evidence | topic feedback; closing time; repeat |
| A_GROUP_STUDY · học nhóm | học/quan hệ; 1–6; LIBRARY | NPC + topic mở | 60; -10; 0 | +10 evidence; shared memory khi event | M03; lỡ lịch; window |
| A_RESEARCH · tra cứu | học/nghề; 2–6; LIBRARY | chủ đề mở | 60; -8; 0 | +5 skill research | mentor lead nếu có; lỡ lịch; repeat |
| A_GROUP_MEET · họp nhóm | xã hội/học; 2–6; CLASS | project nhóm + NPC | 30; -5; 0 | kế hoạch/assignment | M06; conflict chưa tự giải quyết; window |
| A_CLUB_FAIR · thăm hội CLB | khám phá; 2; CLUB | cal | 45; -5; 0 | biết các CLB, chọn đăng ký hoặc không | Y2_CLUB; lỡ fair; once |
| A_CLUB_SESSION · hoạt động CLB | xã hội/nghề; 2–6; CLUB | thành viên + cal | 120; -15; 0 | skill do CLB; quan hệ tùy event | Nam intro; lỡ lịch; window |
| A_CLUB_ORGANIZE · tổ chức | xã hội; 2–6; CLUB | cal + được mời | 90; -12; 0 | project milestone, memory | An event; trễ bài; window |
| A_CAFE_MEET · gặp bạn | xã hội; 2–6; CAFE | NPC + funds | 60; -5; -10 | memory, quan hệ theo event | lời mời; tốn tiền; window |
| A_CAFE_REST · nghỉ quán | cá nhân; 2–6; CAFE | funds | 30; +15; -5 | hồi E | gặp NPC tùy lịch; tốn tiền; repeat |
| A_CAFE_SHIFT · ca quán | việc; 2–6; CAFE/CANTEEN | hợp đồng + cal | 180; -25; +40 | tiền khi hoàn tất ca | vấn đề ca làm; lỡ học; shift |
| A_TUTOR · dạy kèm | việc; 2–6; WORK | hợp đồng + cal | 120; -15; +35 | tiền khi hoàn tất; evidence tùy event | học viên; lỡ học; shift |
| A_GROUP_PROJECT · làm nhóm | học/nghề; 2–6; PROJECT | project active + NPC | 90; -15; 0 | milestone; quality theo project | M07; blocker; repeat |
| A_PERSONAL_PROJECT · project cá nhân | nghề; 2–6; ROOM/PROJECT | skill prerequisite | 90; -15; 0 | milestone, skill theo task | blocker; deadline; repeat |
| A_HACKATHON · tham gia | nghề/xã hội; 3–6; PROJECT | cal + chấp nhận | 240; -30; 0 | project evidence tùy outcome | hackathon; trùng lịch; once/event |
| A_PROJECT_REVIEW · review project | nghề; 2–6; PROJECT | artifact + reviewer available | 30; -4; 0 | feedback, không tự tăng quality | N03; reviewer từ chối; window |
| A_CAREER_FAIR · hội nghề | nghề; 3–6; CAREER | cal | 90; -10; 0 | biết JD, recruiter | N08; trùng lịch; once/event |
| A_CV_REVIEW · sửa CV | nghề; 3–6; CAREER | evidence hoặc CV draft | 45; -5; 0 | cập nhật CV snapshot tiếp theo | N09; không bảo đảm pass; repeat |
| A_MOCK_INTERVIEW · luyện phỏng vấn | nghề; 3–6; CAREER | cal/reviewer | 60; -10; 0 | feedback và practice evidence | mock; trùng lịch; window |
| A_APPLY · ứng tuyển | nghề; 3–6; ROOM | JD active + điều kiện | 30; -5; 0 | application snapshot | screening; có thể bị từ chối; once/attempt |
| A_INTERVIEW · phỏng vấn | nghề; 3–6; WORK | invitation + cal | 60; -10; 0 | kết quả theo company | reject/offer; deadline; once/attempt |
| A_INTERNSHIP_SHIFT · ca thực tập | nghề; 4–6; WORK | hợp đồng + cal | 240; -30; +60 | milestone, thu nhập theo ca | bug/review; trùng lớp; shift |
| A_EXAM · làm bài thi | học; 1–6; CLASS | đăng ký + window | theo assessment; E rule author; 0 | điểm; submit một lần | result; fail vẫn có recovery nếu author; once/attempt |
| A_CALL_HOME · gọi gia đình | quan hệ; 1–6; ROOM | contact available | 30; -2; 0 | memory/thoại theo event | family callback; lỡ lịch; window |

## Gaps trước khi đưa vào production

Các event được gọi tên là placeholder, không phải event record. Giá trị trả lương/chi phí/evidence chỉ để thử, không suy ra cân bằng. Chưa có route graph đầy đủ, menu, giá, lịch NPC, mô tả interruption từng activity hoặc quyền truy cập cho mọi năm; A_EXAM chưa có chi phí cụ thể. Cần chuẩn hóa rủi ro, effect, unlock, relation, skill thành schema machine-readable và kiểm tra tài nguyên tối thiểu để không khóa mềm.
