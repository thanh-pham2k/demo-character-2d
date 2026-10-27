# Checkpoint nhanh — T-001 / Năm 1 v3

Phạm vi: `mobile-2d/Assets/Scenes/Room404_MVP.unity`. Không phải checkpoint mặc định của `Production_Opening`.

Trong Unity mở **KTX404 → Checkpoint nhanh (F8)**, tìm tên hoặc ID rồi bấm **Nạp & Play**. Nút chỉ chạy khi người dùng bấm; mỗi lượt bắt đầu phiên mới để dọn coroutine, reservation, modal, pose và actor cũ. Save riêng nằm trong `Application.temporaryCachePath/Room404-ManualCheckpoints/<id>/<lượt>/year-one-v3.json`.

**Tiếp tục sandbox gần nhất** khởi động lại phiên và đọc lại save sandbox vừa chơi, không dựng lại fixture ban đầu. Dùng nút này để kiểm tra load giữa thi hoặc sau một lựa chọn. Bấm Nạp & Play của một entry sẽ tạo một sandbox mới. Để trở lại save chính: Stop → Play bình thường → Tiếp tục. Save v1/v2 được giữ nguyên; T-001 dùng v3 và cần New Game nếu chưa có v3.

Tất cả mục dưới đây là **kết quả mong đợi, chưa được kiểm tra runtime trong lần triển khai T-001**.

| Checkpoint (ID / tên tìm F8) | Thao tác | Kết quả mong đợi |
|---|---|---|
| `arrival` / Nhận phòng | Tới quầy, thử đưa balo rồi nộp giấy; nhận khóa; đi sang phải lên tầng 4 | Bắt đầu tại quầy, không ở phòng 404 như ảnh lỗi. Balo bị từ chối; giấy → khóa → hành lang, chưa tự bước vào phòng |
| `arrival-hallway` | Tới cửa 404, [E] dùng khóa, đi qua ngưỡng cửa | Chưa mở khóa thì chưa vào được. Mở khóa và bước vào là hai tiến độ riêng; load sau lên tầng vẫn ở hành lang |
| `arrival-before-look` | [E] quan sát phòng ở gần cửa → tới marker chọn giường | Chưa quan sát thì chưa chọn giường được; sau quan sát mới mở lựa chọn dãy/tầng |
| `arrival-room` / Trước chọn giường | Tới marker giường, chọn dãy trái/phải rồi tầng | Lựa chọn được lưu. Không tự hoàn tất đặt balo, dọn hoặc làm quen |
| `bed-level-True` / Giường tầng trên | E rời giường, E nằm lại | Có tiếp cận thang và frame leo lên/xuống; giữ đúng pose/foreground |
| `bed-level-False` / Giường tầng dưới | E rời/nằm lại | Nằm ở tầng dưới, không chạy frame leo. Kiểm tra contact và không bị cắt bởi railing |
| Hai entry giường trên/dưới | Đặt balo bên giường, tới chỗ đồ/rác dọn ba lượt, làm quen các bạn | Chỉ sau đặt balo mới dọn được; chỉ dọn xong mới giới thiệu; sau đó đi căn tin chọn bữa và về phòng |
| `canteen-menu`, `canteen-selected` | Chọn từng món → Quay lại đổi món → Ăn; tiếp tục sandbox trước/sau Ăn | Chọn món chưa trừ tiền/chưa hoàn tất bữa. Ăn mới trừ 25.000đ và nhận hỗ trợ một lần; chưa ăn load ở căn tin, sau ăn không thanh toán/nhận hỗ trợ lần nữa |
| `canteen-eaten` | Tới lối ra căn tin → về tầng 4 → qua cửa 404 | Người chơi tự trở về phòng; tiếp tục sandbox tại căn tin không thu tiền hoặc cấp hỗ trợ lần nữa |
| `arrival` / `morning-after-brush` | Nộp giấy tại quầy / đánh răng tại WC → xem thông báo hướng dẫn | Thông báo hiện ở quầy và hành lang, chỉ dẫn nhận chìa khóa hoặc rửa mặt tiếp |
| `morning-routine` / Ngày 1: báo thức và routine | E thức dậy → mở cửa/ra WC → E đánh răng → E rửa mặt → về cạnh giường thay đồ → ngồi mở laptop/LỊCH HỌC → đứng dậy lấy sách → lấy balo | Alarm dừng khi thức dậy; vệ sinh lưu hai bước riêng. Xem lịch là hành động trên laptop, không tự ghi khi lấy sách. Chưa rửa mặt thì chưa thay đồ; chưa xem lịch thì chưa lấy sách; chưa có sách thì chưa đeo balo |
| `morning-after-brush` | Thử xuống đi học; sau đó E rửa mặt rồi về phòng | Chưa rửa mặt thì chưa đủ điều kiện. Load giữ brush, không bắt đánh răng lại; rửa mặt xong mới tiếp tục thay đồ |
| `morning-routine` | Ra hành lang/đánh răng → tiếp tục sandbox; sau đó quay vào 404 → tiếp tục sandbox lần nữa | Load lần đầu ở hành lang với brush đã ghi; lần sau ở 404. Lỗi lưu portal giữ phía nguồn, không chuyển người chơi qua cửa rồi mới báo lỗi |
| `HK1_FIRST_CLASSES` / Ngày học đầu tiên | Đi xe, vào H6, ngồi; học GT1 rồi ra cổng/vào lớp VL1 | 08–10 GT1, 10–12 VL1; điểm danh trước câu hỏi; đúng giờ/trễ theo clock thật; rời lớp trở lại H6 đúng vị trí |
| `class-check-in`, `class-check-in-late` | Vào lớp/ngồi, chờ rồi bấm Điểm danh; thử Rời chỗ → hủy/xác nhận | Ngồi chưa tự điểm danh. Sau khi bấm mới ghi đúng giờ/trễ và học. Rời chỗ giải phóng pose, không tự nhận topic; attendance đã ghi được giữ |
| `classroom-before-seat` | Tiếp tục sandbox → ngồi cạnh bạn, điểm danh; rời lớp rồi tiếp tục sandbox | Load trong đúng lớp trước khi ngồi; sau khi rời lớp load ở cổng H6 |
| `classroom-seated` | Tiếp tục sandbox khi đang học → đợi hoạt động | Giữ pose ngồi và điểm danh; còn khoảng 10 phút game trước câu hỏi, không học lại 15 phút từ đầu |
| `classroom-question` | Tiếp tục sandbox giữa câu hỏi; trả lời sai rồi tiếp tục sandbox lần nữa | Giữ câu hỏi, khoảng 11 giây còn lại và pose ngồi; sau trả lời sai giữ trạng thái chờ Thử lại |
| `choice-morning` | Chọn đi học hoặc chơi; nếu chơi, ngồi mở laptop/game rồi đợi gần tiết | Có lời rủ trước lớp. Cảnh báo 30/15/5 phút; có thể dừng đi học hoặc tiếp tục và bị trễ/vắng theo giờ |
| `choice-before-schedule` | Ngồi vào máy và thử game trước khi xem lịch; xem lịch, đứng dậy nghe lời rủ rồi chọn chơi | Không bắt đầu game trước lời rủ; sau khi chọn vẫn có thể chơi và dừng để đi học |
| `reminder-30`, `reminder-15`, `reminder-5` | Tiếp tục game rồi chọn thoát | Clock game x4; cảnh báo cập nhật và không cộng stack. Thoát trả x1, không tự dịch chuyển tới lớp |
| `first-bus-time` / `reminder-30` | Xem nhắc giờ ở trạm và khi đang chơi game | Khi không chơi chỉ hiện Đã hiểu; khi chơi có Thoát game và Chơi tiếp, thoát game trả clock về x1 |
| `choice-evening` | 18:00 chọn học sớm hoặc chơi với Long | Choice riêng với buổi sáng. Học topic thay đổi kiến thức; chơi thực sự cùng Long ghi history/thời gian quan hệ; phần chưa học không tự được hoàn thành |
| `choice-afternoon` | Chơi cùng Long trước 18:00 → dừng, đứng dậy sau 18:00 | Ghi thời gian chơi buổi chiều; lời rủ tối vẫn xuất hiện và lựa chọn tối vẫn riêng |
| `choice-evening` / `coop` | Chơi cùng Long, dừng game rồi trò chuyện cạnh ghế bạn cùng phòng | Thoại phản ánh thời gian chơi thực tế. Thời gian quan hệ không tăng khi Long đang đi tới ghế, đã rời ghế hoặc người chơi đã dừng game |
| `midterm-prepared` / Day 28: đã học sớm | Học tập → chọn môn → Ôn thi → chọn topic | Dashboard thể hiện phần đã nắm/còn thiếu. Topic đã nắm 15 phút game, topic thiếu 45; có hoạt động/câu hỏi |
| `study-in-progress` | Tiếp tục sandbox khi đang ôn → đợi câu hỏi hoặc chọn Dừng học | Giữ ghế/pose và khoảng 10 phút game còn lại; dừng giữ thời gian đã dùng, không tự cấp kiến thức |
| `midterm-gaps` / Day 28: nhiều lỗ hổng | So sánh cùng môn/topic với entry trên, học bù kiến thức rồi ngủ | Phải dành nhiều thời gian hơn; có thể chủ động bù. Không khóa fail chỉ vì từng chọn game |
| `midterm-gaps` | Chọn topic, trả lời sai/hết giờ → Dừng học → xác nhận → chọn topic khác; sau đó thử lại và giải đúng | Sai/hết giờ lưu topic yếu; Dừng học về dashboard, không ép giải đúng và không cấp kiến thức. Thời gian đã dùng giữ nguyên; giải đúng lần sau mới ghi completed và xóa yếu. Âm thanh/pose học cũ được dọn |
| `midterm-gaps`, `class-check-in` | Khi câu hỏi đang chạy, mở Dừng học/Rời chỗ, đợi vài giây rồi Quay lại | Trong confirm, timer và input câu hỏi dừng; hủy confirm tiếp tục câu/timer cũ. Xác nhận dọn puzzle/callback, không tự nhận kiến thức |
| `HK1_MIDTERM_CLASSES` | Đi học, điểm danh hai lớp | Day 29 vẫn VL1 08–10 và NMT 10–12; không bị đổi thành ngày thi |
| `HK1_MIDTERM_EXAM` | Chuẩn bị/đi H6, làm ba môn theo lịch | Day 30 có GT1 08–09, VL1 10–11, NMT 13–14; đề phản ánh topic đã nắm; chưa tăng credits hoặc tạo FAIL course |
| `midterm-active` | Đang câu 2/11s: trả lời tiếp, rồi F8 → Tiếp tục sandbox gần nhất | Dựng lại lớp/ghế, giữ câu/đáp án/timer/preparedness; submit một lần. Strong/neutral/weak và gaps hiện tại bàn |
| `exam-active`, `midterm-active`, `exam2-active` | Thi xong → rời lớp → ra trạm xe về | Fixture có vé đi và safe location ở H6 qua model; có thể tiếp tục chuyến về, không bị kẹt vì thiếu outbound history |
| `deadline-before` / Trước deadline HK1 | 21:59 chọn hoạt động, quan sát 22:00 hoặc ngủ/preview | Bài chưa nộp thành overdue đúng hạn; chưa tự cấp FAIL cả môn trước thi; preview báo hậu quả |
| `deadline-after` | Mở lịch và xem eligibility | Phân biệt bài quá hạn, buổi vắng và kết quả môn. Không thể nộp mới để xóa quá hạn âm thầm |
| `absent`, `makeup` | Xem vắng rồi học bù lần lượt | Có recovery history/topic, mất thời gian; giữ vắng, không biến FAIL thành PASS |
| `makeup-both-lessons` | Chuyển tab Học kỳ → GT1 → học bù buổi 1 rồi buổi 2 | Buổi 2 mở sau khi học bù buổi 1; cả hai lịch sử vắng giữ nguyên, hai topic được học lại |
| `class-check-in` | Vào lớp, điểm danh → chọn rời chỗ trước khi hoàn thành → xem LỊCH HỌC | Ghi đã điểm danh nhưng bài học chưa hoàn thành; không hiển thị đã học đầy đủ |
| `class-near-end` | Vào lớp GT1 lúc 09:55 → ngồi, điểm danh → chờ tới 10:00 | Lưu điểm danh trễ; kết thúc tiết mà không tự mở hoạt động học hoặc cấp kiến thức khi không còn đủ 15 phút |
| `class-check-in` | Điểm danh → rời chỗ trước khi hoàn thành → về 404 và chọn Ngủ trước khi tiết kết thúc | Preview nêu nội dung buổi học chưa hoàn thành, giữ điểm danh và không tự cấp kiến thức; hủy preview giữ state |
| `study-end-of-day` | 23:50 mở topic; quay về lịch học kỳ và thử học bù | Nút ôn/học bù báo không đủ giờ; không thể tiếp tục tích lũy kiến thức vô hạn sau khi clock chạm cuối ngày |
| `homework-repeat-late` | Thử Làm bài tập đã nộp lúc 23:50 | Không đủ 15 phút thì nút bị khóa; không dùng làm lại bài để phục hồi topic miễn phí sau 24:00 |
| `deadline-before` | 21:59 mở Bài tập còn thiếu | Hiện chi phí 15 phút và không đủ giờ trước hạn; không cho bắt đầu hoạt động chắc chắn không thể nộp trước 22:00 |
| `choice-evening`, `coop` | Chơi tới cuối ngày và tiếp tục quan sát | Khi clock chạm 24:00, thời gian chơi cùng Long và session không còn tăng; phải cất balo/tắt đèn nếu được yêu cầu rồi ngủ để chuyển mốc |
| `HK1_PREPARATION` | Vào dashboard, ưu tiên bài tập/học bù/topic rồi ngủ | Attendance, bài, deadline, topic và giữa kỳ đủ để chọn ưu tiên; các hoạt động dùng clock thật |
| `HK1_FINAL_EXAMS` | Đi H6 thi theo ba khung 08–09 / 10–11 / 13–14 | Eligible mới vào bài; 3 câu, 20s/câu; world clock dừng khi exam timer chạy |
| `exam-active` | Nộp sớm/thoát → hủy hoặc xác nhận; tiếp tục sandbox sau trả lời | Hủy tiếp tục attempt; xác nhận tính câu trống sai; load giữ câu/timer; 2/3 PASS, không có retry miễn phí hoặc credits lặp |
| `exam-ineligible` | Đến lớp hoặc ngủ qua kỳ thi | Hiện lý do eligibility; hết window ghi FAIL đúng reason. Không thể nhấn hoàn tất để PASS |
| `exam-ineligible`, `final-0`, `final-1`, `final-2` | Để đủ ba kết quả, kể cả không có bài thi nào được nộp; đọc/xác nhận recap, reload sandbox | Recap vẫn hiện đủ PASS/FAIL/reason/credits/nợ. Xác nhận được lưu; reload không mở lại recap đã đọc. Chưa xác nhận recap Năm 1 thì chưa bắt đầu lẩu |
| `HK1_FINAL_EXAMS-skip`, `HK2_FINAL_EXAMS-skip` | Ngủ qua các window thi chưa dự; đọc summary sau commit | Summary ghi kết quả và lý do từng môn ngoài cảnh báo bỏ thi. HK2 vẫn ở ngày cuối để đọc recap/chuẩn bị lẩu, chưa tự hoàn tất năm |
| `HK2_START` | Đi routine đầy đủ từ 06:40 tới trạm | Có thời gian tới xe 07:40; không dùng nhãn 06:40 nhưng model vẫn 07:20 |
| `bus-before`, `missed-bus` | Lên chuyến đầu hoặc đợi chuyến sau | Nhánh kịp/lỡ, fare và arrival đúng. Xe sau không tự biến arrival thành điểm danh đúng giờ |
| `HK1_FIRST_CLASSES` | Từ hành lang xuống sảnh → tiếp tục sandbox; đi xe về rồi lên tầng → tiếp tục sandbox | Xuống sảnh lưu Campus; lên tầng lưu Hallway. Load không nhảy sai phía cầu thang; save lỗi không chuyển vùng hoặc trừ vé |
| `first-bus-time` | 07:30 tại trạm: chờ xe ghé, thử E trước/sau 07:40 | Trước 07:40 bị nhắc chờ, chưa trừ tiền. Từ 07:40 mới mua vé/đi; giờ lớp vẫn 08:00 |
| `first-bus-time` | Ở trạm → tiếp tục sandbox gần nhất; sau đó đi về phía sảnh → tiếp tục sandbox | Load tại trạm sau lần đầu, tại sảnh sau khi rời trạm; chưa trừ vé khi chưa lên xe |
| `HK2_DORM_LIFE`, `coop` | Học VL2/Lập trình, về phòng rồi chơi với Long | Day 155 VL2 08–10, Lập trình 10–12; hai người dùng ghế riêng; 20s không ép thoát phiên chơi |
| `lights-late` / Long muốn ngủ lúc 23h | Chọn dừng hoặc tiếp tục; đứng dậy/cất balo, tới công tắc tắt đèn rồi tới giường ngủ | Phản ứng được lưu nhưng không tự tắt đèn. Còn đèn theo lời nhắc thì chưa chuyển ngày; reload chưa chọn phản ứng vẫn hiện lựa chọn |
| `lights-late` | Tắt đèn → F8 tiếp tục sandbox gần nhất | Trở lại phòng với đèn đã tắt; không yêu cầu tắt lần nữa. Nếu load trước tắt đèn, vẫn dùng được công tắc dù trước đó chưa chơi cùng Long |
| `HK2_PREPARATION`, `deadline2-before` | Xem lịch GT2 08–10/Lập trình 10–12 và dashboard HK2; chọn việc trước 22:00 | Hai buổi 2 giữ từ course graph; đi học hoặc ưu tiên việc khác có attendance/vắng theo giờ. Bài nộp/overdue và eligibility được giữ |
| `HK2_FINAL_EXAMS`, `exam2-active` | Làm ba môn hoặc tiếp tục sandbox giữa bài thi | GT2 08–09; VL2 10–11; Lập trình 13–14; phục hồi attempt và tính credit một lần |
| `final-0`, `final-1`, `final-2` | Xem recap cuối năm | Các tuyến 0/26/32 TC được dựng qua model; lý do môn nợ hiện rõ; vẫn được dự lẩu |
| `feast` | Tới bếp đặt bếp → nguyên liệu bên trái → chỗ ngồi bên phải → tới bàn gọi bạn → về bếp ăn | Bốn bước riêng có tiến độ lưu; dialogue đổi theo kết quả; chưa đánh hoàn tất năm khi chỉ đang ăn |
| `feast-prep-1`, `feast-prep-2`, `feast-prep-3` | Xem nồi/nguyên liệu/bốn chỗ ngồi theo checkpoint rồi làm bước còn thiếu; tiếp tục sandbox | Đạo cụ tương ứng xuất hiện theo cờ đã lưu; chưa hiện cả nhóm ăn trước khi chuẩn bị xong. Mỗi bước có thao tác/thời lượng riêng, save lỗi không công bố bước mới |
| `after-feast` | Nằm nghỉ, xem preview, hủy; sau đó chọn ngủ/commit | Hủy giữ sau lẩu; commit tới ngày 212 07:20, snapshot giữ results/debt/history/topics/giường/quan hệ; không bật gameplay Năm 2 demo |
| `year2-0`, `year2-1`, `year2-2` | Xem snapshot bàn giao, reload sandbox | Giữ 0/26/32 và nợ tương ứng, không tự thêm tín chỉ hoặc chạy lại lẩu |
| Bất kỳ entry `*-skip` | Ngủ → preview → hủy / commit / tiếp tục sandbox | Nguồn, đích, duration đúng; chỉ xử lý event đã author; không tạo buổi/vắng trong khoảng trống; nhiều tuần có montage; commit không lặp |
| `HK1_GAMING_CHOICE-skip` | Chưa xem lời rủ buổi tối: ngủ, hủy rồi xác nhận | Preview báo lời rủ bị lỡ; hủy giữ nguyên history. Xác nhận ghi invitation skipped, không tự nhận kiến thức/thời gian chơi |

Đổi checkpoint giữa lúc game/thi/ngủ để quan sát cleanup: phiên mới phải không giữ coroutine, actor reservation, modal hoặc pose của phiên trước. Các timer vẫn chạy khi đã vào checkpoint; dùng nút Pause của Unity nếu cần quan sát.

Trước khi ngủ: nếu còn đeo balo, E rời giường → tới bàn cất balo → nằm lại. Có thể cất balo cả khi chưa đi xe; thao tác này không bị khóa vào một chuyến về. Day 0 sau đặt balo phải thấy balo tại chỗ ngủ đã chọn; khi đi học outfit phải thể hiện balo, khi cất móc treo được cập nhật.

Các entry trong nhóm `12 milestone` bắt đầu tại giờ mốc: Day 0 ở quầy; các ngày sau nằm trên giường đã chọn, chưa thức dậy/vệ sinh/thay đồ/lấy sách. Chọn entry → thức dậy và đi theo routine → giờ, pose và hướng dẫn khớp mốc. Các fixture riêng vẫn đặt tiến độ tương ứng với nhánh cần kiểm tra.

