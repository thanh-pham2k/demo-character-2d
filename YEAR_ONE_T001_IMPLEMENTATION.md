# T-001 — Năm 1, lịch 12 mốc và state v3

Cập nhật 26/10/2026. Scene: `mobile-2d/Assets/Scenes/Room404_MVP.unity`.
Trạng thái: **đã bổ sung/rà source theo file flow; chờ manual, chưa nghiệm thu**. Ảnh người dùng phản bác kết luận hoàn tất trước đó: HUD ở bước nộp giấy nhưng nhân vật đứng trong 404. Khi rà ảnh, scene có `routineCatalog: {fileID: 0}` làm `SetupArrival` return sớm. Đã gán lại reference, nối vào scene builder và thêm lookup Editor để scene đã mở với reference rỗng không âm thầm bỏ qua quầy; build vẫn yêu cầu serialized catalog. Chưa compile, Play Mode, test suite hoặc build theo AGENTS.md. Kết quả kiểm tra v2 cũ không chứng minh các thay đổi này.

## Sai lệch step đang sửa

- Day 0: trước đây thao tác lên tầng tự đánh enter-room và teleport vào phòng; nay lên hành lang → dùng khóa → bước qua cửa → quan sát phòng → chọn giường là các tiến độ riêng. Fixture mới `arrival-hallway`, `arrival-before-look` bao phủ các ranh giới này.
- Routine: trước đây xem lịch/lấy sách là một thao tác tại station; nay phải ngồi mở laptop và xem tab lịch, rồi đứng dậy lấy sách trước khi đeo balo.
- WC: trước đây một routine ghi đồng thời brush/face. Nay tương tác đánh răng xong phải tương tác rửa mặt tiếp; chỉ bước face mới đánh dấu refreshed. Có checkpoint `morning-after-brush` cho save/restore giữa hai bước. Fixture author vẫn dùng action wash để dựng trạng thái đã hoàn thành cả hai; runtime T-001 dùng từng bước.
- Căn tin: tách chọn món (lưu `dinnerChoice`, không thanh toán) khỏi xác nhận Ăn (transaction thanh toán/meal/hỗ trợ một lần). Vào căn tin lưu safeLocation Canteen; restore dựng lại căn tin/menu nếu chưa ăn. Hai nút món truyền hai index riêng. Thêm `canteen-menu` và `canteen-selected`.
- Lẩu: model và tương tác đặt bếp yêu cầu đã cất balo tại phòng; HUD chỉ dẫn cất balo trước. Không dùng kết quả thi xong làm bằng chứng đã thực hiện routine quay về/cất đồ.
- Lớp: ngồi chưa tự ghi attendance; nút Điểm danh dùng khung giờ thực tế rồi mới bắt đầu nghe/ghi và puzzle. Có xác nhận rời chỗ trước hoạt động hoàn tất, giữ attendance nhưng chưa cấp topic; save lỗi sau giải đúng có đường thử lại. Thêm fixture `class-check-in`/`class-check-in-late`. Chưa kiểm tra UI/timing runtime.
- Đối chiếu file đính kèm `9581d068-d1ee-4dc1-b748-f4cf2366dbc9/Pasted text.txt`: chuẩn bị lẩu có đạo cụ nồi/túi nguyên liệu/bốn khay hiện theo từng bước lưu, không chỉ thông báo; mỗi bước có thời lượng và transaction. Chỉ nhận sprite sẵn có, thiếu reference báo lỗi thay vì dựng ảnh rỗng.
- Đèn Day 155 có lựa chọn phản ứng dừng/tiếp tục được lưu riêng; cả hai nhánh vẫn cần tự đứng dậy/đi tới công tắc để tắt đèn. Load giữa lời nhắc và lựa chọn không bỏ mất lựa chọn. Thoại lẩu phân biệt đủ 32 TC, nợ một phần và chưa đạt cả 6 môn; không đổi academic rule.
- Xe đầu tất cả ngày học là 07:40; model/boarding/UI cùng kiểm tra timestamp. Fixture ở cổng/giữa thi đã author chuyến đi từ 07:40. `first-bus-time` kiểm tra chờ xe trước giờ; Day 113 vẫn giữ nhánh chuyến sau riêng.
- Balo đặt Day 0 có sprite ở chỗ ngủ đã chọn; trước đó nhân vật mang balo bằng pose có sẵn. Đeo balo đổi outfit về bộ có balo; cất cập nhật hook. Ngủ yêu cầu cất balo; có thể cất tại phòng kể cả khi chưa đi xe để không tạo ngõ cụt.
- Dashboard hiển thị eligibility thực tế, lý do attendance/bài tập và thời gian còn tới hạn; mặc định chọn đúng học kỳ. Có tương tác xem lại phòng/vật dụng trong free time. Source không chứng minh layout, contact hoặc animation đã đạt: các mục này vẫn chờ manual.
- Kết quả ôn/bài tập đúng nhưng save lỗi có modal thử lưu lại, không bắt làm lại hoạt động để lưu cùng kết quả. Clock game/quan hệ chỉ tính phần thời gian thật còn lại trong ngày; không cộng vô hạn sau 24:00.
- Rà tiếp nhánh weak topic phát hiện puzzle chỉ cho retry, chưa có đường quay về dashboard. Đã thêm Dừng học khi đang học/đang trả lời/sai/hết giờ: confirm hủy coroutine/callback, giữ thời gian và weak topic, không cấp completed topic. Kết thúc/hủy dọn pose/âm thanh học. Trong lớp, nút Rời chỗ cũng còn dùng được khi puzzle mở.
- Puzzle có callback pause riêng cho confirm/startup/lỗi lưu. Trong confirm hủy hoạt động, timer và input câu hỏi dừng; không dùng `WorldPaused` trực tiếp vì world vốn dừng suốt thời gian puzzle. Caller khác mặc định không đổi khi callback null.
- Rà nhánh không thi/bỏ thi: recap trước đây chỉ đi kèm kết quả vừa nộp. Nay `RecapReady` dựa vào đủ kết quả học kỳ, gồm FAIL do không eligible/bỏ thi; có modal tổng kết, acknowledgment lưu riêng và gate trước chuẩn bị lẩu. Fixture cuối năm/after-feast author acknowledgment trước PrepareFeast. Thông báo không vào được kỳ thi phân biệt lỗi lưu, thiếu attendance/bài tập và kỳ thi đã ghi nhận.
- Nếu ngủ qua kỳ thi, summary sau transition cũng liệt kê PASS/FAIL/reason từng môn và ghi nhận đã đọc recap của mốc nguồn; không phụ thuộc người chơi phải nộp một bài thi trước. Body modal tự co chữ 14–21 để giữ recap nhiều dòng trong vùng hiện có; layout chưa manual.
- Portal ngày thường trước đây không cập nhật safeLocation, khiến load giữa WC quay về phòng dù fixture hallway đúng. Nay commit portal lưu Hallway/Room trước khi đổi vùng; vào phòng lần đầu vẫn kiểm tra khóa/arrival. Save lỗi giữ phía nguồn. Checkpoint hướng dẫn thêm reload thật giữa WC, không chỉ fixture author sẵn Hallway.
- Cầu thang T-001 commit Campus/Hallway trước khi đổi vùng, giữ phía nguồn nếu save lỗi. Không thay đổi host production hoặc legacy; chỉ chạy transaction khi campaign YearOne đang active.
- Bài tập cũng kiểm tra đủ 15 phút còn lại, kể cả làm lại bài đã nộp; bài mới phải hoàn tất trước deadline hiện hành. UI nêu chi phí/lý do chưa thể bắt đầu. Thêm fixture `homework-repeat-late`.
- Nghe/ghi bài, ôn topic và làm bài tập chờ timestamp đích trên `absoluteMinutes`; học bù nhận phần clock đã tiến trong frame. Không dùng một timer Time.deltaTime riêng để cấp tiến độ trong khi clock game dừng hoặc đã chạm cuối ngày. Timer 20 giây câu hỏi/thi vẫn là cơ chế riêng như thiết kế.
- Các mô tả “đã triển khai” bên dưới chỉ mô tả source hiện có, không xác nhận runtime đã nghiệm thu. File flow người dùng gửi đã được đối chiếu theo các nhóm bước dưới đây; gameplay, UI và animation vẫn cần manual.

## Đối chiếu 12 mốc với file người dùng gửi

Nguồn: `/Users/thanhpv/.codex/attachments/9581d068-d1ee-4dc1-b748-f4cf2366dbc9/Pasted text.txt`. Mỗi dòng mô tả code/state đã nối, không phải ca chạy PASS.

| Mốc | Thao tác/nhánh trong source | F8 để nghiệm thu |
|---|---|---|
| Day 0 | Quầy/giấy/khóa → hành lang → mở khóa → bước vào → quan sát → dãy/tầng giường → đặt balo/dọn/làm quen → chọn món/xác nhận ăn → free time/ngủ | `arrival`, `arrival-hallway`, `arrival-before-look`, `bed-level-*`, `canteen-*` |
| Day 1 | Báo thức/ra giường → brush/face riêng → thay đồ → laptop lịch → lấy sách → balo → xe 07:40 → vào lớp/ngồi/điểm danh/nghe/puzzle → về/cất balo/ngủ | `morning-routine`, `morning-after-brush`, `first-bus-time`, `class-check-in` |
| Day 2 | Lời rủ sau xem lịch; học/game trước lớp, reminders; choice buổi tối riêng, knowledge và thời gian cùng Long; preview lời rủ bị bỏ lỡ | `choice-morning`, `choice-evening`, `reminder-*`, `HK1_GAMING_CHOICE-skip` |
| Day 28 | Môn/topic, thời lượng theo gap, sai/hết giờ ghi weak; đúng ghi completed; giới hạn thời gian và save retry | `midterm-prepared`, `midterm-gaps`, `study-end-of-day` |
| Day 29 | VL1 08–10/NMT 10–12 qua cùng flow lớp, attendance theo lúc bấm và kiến thức sau hoàn tất | `HK1_MIDTERM_CLASSES` |
| Day 30 | Ba bài giữa kỳ riêng; preparedness từng topic, strong/neutral/weak; không credits/course PASS | `HK1_MIDTERM_EXAM`, `midterm-active` |
| Day 92 | Dashboard assignment/deadline/eligibility/topic; bài tập/học bù/ôn dùng quỹ thời gian, overdue 22:00 | `HK1_PREPARATION`, `deadline-*`, `makeup-both-lessons` |
| Day 99 | Routine thi, 3 câu × 20s, timer riêng/world pause, confirm nộp sớm, reload attempt, result/credit một lần | `HK1_FINAL_EXAMS`, `exam-active`, `exam-ineligible` |
| Day 113 | Routine trước 07:40, kịp/lỡ và chuyến sau, GT2/VL2, về/cất balo | `HK2_START`, `bus-before`, `missed-bus` |
| Day 155 | VL2 08–10/Lập trình 10–12, học/game/talk, phản ứng đèn dừng/tiếp tục rồi tự tới công tắc | `HK2_DORM_LIFE`, `coop`, `lights-late` |
| Day 204 | Giữ hai buổi GT2/Lập trình; ưu tiên bài/học bù/topic; deadline 22:00 và preview | `HK2_PREPARATION`, `deadline2-before` |
| Day 211 | Thi/recap → về/cất balo → nồi/nguyên liệu/bốn chỗ/gọi bạn → lẩu theo kết quả → ngủ commit snapshot 212 07:20 | `exam2-active`, `final-*`, `feast`, `feast-prep-*`, `after-feast`, `year2-*` |

Asset của các phần bổ sung dùng catalog/scene hiện có: balo/outfit, nồi, `hotpot_supply_bag`, `meal_tray`, `food_meat`, `food_vegetables`. Đã xem cutout túi/khay để xác nhận loại đạo cụ; không generate ảnh mới vì chưa thiếu asset trong các nhóm này. Bố cục/cỡ/sorting trong scene vẫn cần manual.

## Lịch đã author

| Ngày lịch | Milestone | Giờ bắt đầu | Lịch tương tác |
|---:|---|---|---|
| 0 | YEAR1_ARRIVAL | 14:20 | Quầy KTX → giấy → khóa → lên 404 → chọn dãy/tầng giường → đặt balo → dọn → làm quen → ăn |
| 1 | HK1_FIRST_CLASSES | 06:40 | GT1 08–10; VL1 10–12 |
| 2 | HK1_GAMING_CHOICE | 06:40 | NMT 08–10; GT1 10–12; lời rủ trước lớp và lúc 18:00 |
| 28 | HK1_MIDTERM_PREP | 07:00 | Chọn môn/topic, ôn phần còn thiếu |
| 29 | HK1_MIDTERM_CLASSES | 06:40 | VL1 08–10; NMT 10–12 |
| 30 | HK1_MIDTERM_EXAM | 06:30 | GT1 08–09; VL1 10–11; NMT 13–14 |
| 92 | HK1_PREPARATION | 07:00 | Bài tập/học bù/ôn; hạn bài 22:00 |
| 99 | HK1_FINAL_EXAMS | 06:30 | GT1 08–09; VL1 10–11; NMT 13–14 |
| 113 | HK2_START | 06:40 | Xe đầu 07:40; GT2 08–10; VL2 10–12 |
| 155 | HK2_DORM_LIFE | 06:40 | VL2 08–10; Lập trình 10–12; Long và đèn buổi tối |
| 204 | HK2_PREPARATION | 07:00 | GT2 buổi 2 08–10; Lập trình buổi 2 10–12 (giữ course graph); bài tập/học bù/ôn; hạn bài 22:00 |
| 211 | HK2_FINAL_EXAMS | 06:30 | GT2 08–09; VL2 10–11; Lập trình 13–14; lẩu |

Sau lẩu, ngủ bàn giao snapshot lúc 07:20 ngày 212. Không bật stage demo Năm 2/Năm 5.

## Các thay đổi source

- `YearOneCalendar` có StartMinute cho mỗi milestone và sessions giữa kỳ riêng, không gộp với finals. Course ordinals được cập nhật để Day 29 giữ đúng hai buổi.
- `YearOneProgress` vẫn sở hữu absoluteMinutes, tiền, academic results và history. Partial `YearOneAcademicTopics` thêm ba topic/môn, completed/weak topics, kết quả giữa kỳ và preparedness snapshot cho từng câu thi. Đề có mức khác nhau theo topic đã nắm; đúng/sai do đáp án người chơi, không ép fail.
- Giữa kỳ trả điểm và strong/neutral/weak theo topic, không cấp tín chỉ. Bỏ giữa kỳ giữ kiến thức đã học, chỉ ghi thiếu kết quả. Finals giữ điều kiện attendance/homework và ngưỡng 2/3.
- UI tại bàn cho chọn môn/topic. Ôn topic đã nắm: 15 phút game; topic thiếu: 45 phút game, có câu hỏi/feedback. Học bù giữ lịch sử vắng, không sửa kết quả đã chốt.
- `YearOneArrival` dựng vùng quầy từ background/manager trong catalog hiện có; input nộp giấy, nhận khóa, đi tới lối lên 404 dùng state/transaction hiện tại.
- `YearOneDailyRoutine` nối station vào vị trí prop sẵn có. Mỗi ngày ngủ dậy bắt đầu tại giường đã chọn; âm báo thức dừng khi thức dậy. Vệ sinh ở WC được chia nhãn đánh răng/rửa mặt; sau đó trở về thay đồ, xem lịch/lấy sách và đeo balo. Có thao tác cất balo khi trở về.
- `BedInteractable` có admission theo giường chọn và tầng trên/dưới. Tầng trên dùng pose leo có sẵn; tầng dưới chuyển sang pose nằm, không chạy frame leo. Vị trí tầng dưới lấy từ chiều cao bed art; cần duyệt contact/occlusion thủ công.
- `YearOneFeastPreparation` lưu lần lượt bếp → nguyên liệu → chỗ ngồi → gọi bạn trước khi ăn. Thoại thay theo tín chỉ/môn nợ. Lẩu không yêu cầu 32 TC.
- Sleep preview hiện nguồn/đích, duration và hậu quả; đoạn nhiều ngày có thẻ montage tự chuyển trước summary. Chỉ `Sessions`/deadline đã author được xử lý, không sinh lịch học trong khoảng trống.
- Restore dọn coroutine của flow, modal, desk, companion, interaction và dựng safe location. Bài thi dở được dựng lại ở lớp/ghế, giữ câu/đáp án/preparedness/timer.
- Rà nguồn bổ sung: restore gọi `CancelActiveTrip` và `CancelActiveJourney` để ngừng coroutine xe/WC, khôi phục actor và giải phóng transition trước khi dựng phiên mới. Reset daily flags xóa `ReadyForBus`; disable flow xóa cờ hoạt động và dừng báo thức/puzzle. Đây chưa phải bằng chứng runtime.
- Recovery học bù buổi 2 chấp nhận buổi 1 đã học bù, không yêu cầu ghi lại attendance buổi 1. Lịch sử vắng vẫn được giữ.
- Thời gian chơi cùng Long chỉ tăng khi session game đang chạy và companion đang ở pose Gaming, không dựa vào cờ lựa chọn lịch sử. Modal trả nhãn hủy về “Quay lại” trước khi gán nhãn lựa chọn riêng; preview ngủ nêu địa điểm nguồn/đích và giới hạn của học bù.
- Tương tác trò chuyện cạnh ghế roommate có thoại theo thời gian chơi cùng Long/topic yếu. Restore Day 155 xử lý cả nhánh argument không có game trước đó và áp lại trạng thái đèn đã tắt; đồng thời xóa hội thoại/academic/lẩu và callback tạm của phiên cũ.
- Khởi động ôn topic/học bù kiểm tra quỹ thời gian còn lại tới cuối ngày; UI nêu chi phí và vô hiệu hóa khi không đủ giờ. Có fixture `study-end-of-day` và `makeup-both-lessons` để nghiệm thu hai trường hợp biên, chưa chạy.
- Restore hủy cả coroutine lên/xuống giường trước khi dựng pose mới; F8 tìm được theo ID entry như hướng dẫn. Các phần này mới rà call site/source.
- Study puzzle có callback sai/hết giờ tùy chọn để ghi weak topic; giải đúng ghi completed topic và xóa yếu. Các caller bài học/bài tập cũ giữ hành vi mặc định; cancel/disable xóa cả hai callback.
- Rà schedule phát hiện slot Day 155 còn đảo từ baseline: đã đổi VL2 buổi 2 về 08–10 và Lập trình buổi 1 về 10–12 theo graph. Fixture exam-active/midterm-active/exam2-active ghi chuyến đi qua `CommitTrip(false)` để có đường xe về sau thi.
- Day 204 giữ hai buổi GT2/Lập trình có sẵn trong course graph, đã ghi rõ vào graph thay vì bỏ lịch âm thầm. Ngủ sớm Day 2 preview lời rủ chưa xem; commit lưu `invitation:<phase>:skipped`, không tự nhận knowledge hoặc thời gian chơi.

- Day 0 sau khi ăn lưu vị trí căn tin, cho người chơi tự chọn lối ra về tầng 4 rồi đi qua cửa 404; fixture `canteen-eaten` kiểm tra nhánh này. Hướng dẫn HUD phân biệt căn tin, hành lang và thời gian tự do.
- Fixture `choice-morning` dừng trước bước lấy sách; `choice-evening` ghi chuyến đi, hai tiết, chuyến về và cất balo. HUD sau khi dừng game hướng tiếp tới sách và không nhầm điểm danh với bài thi. Các thay đổi này chưa được chạy trong Unity.

- Thông báo hành động của hành trình được phép hiển thị tại quầy, căn tin, hành lang và phòng; chỉ ẩn khi thoại, bảng học vụ hoặc bữa lẩu đang chiếm màn hình. Cần kiểm tra vị trí và khả năng đọc trong Unity.

- F8 nhóm `12 milestone` dựng trạng thái đầu ngày thay vì đánh dấu sẵn toàn bộ routine và đeo balo; các fixture ôn giữa kỳ/bài tập muộn tại bàn đã cất balo. Cần xác nhận pose và thời gian bằng manual.

- Lịch phân biệt đã điểm danh với đã hoàn thành bài học. Học bù yêu cầu thức dậy, thay đồ, xem lịch, ở phòng và đủ thời gian; UI dùng cùng điều kiện với hành động để tránh nút có thể bấm nhưng không bắt đầu. Chưa kiểm tra runtime.

- Buổi học kiểm tra thời gian còn lại trong khung tiết: điểm danh sát giờ vẫn được lưu, nhưng nếu không đủ 15 phút cho hoạt động thì kết thúc khi hết tiết và chưa cấp nội dung. Có F8 `class-near-end`; chưa kiểm tra runtime.

- Preview ngủ báo nội dung tiết đã điểm danh nhưng chưa hoàn thành nếu chuyển mốc trước khi tiết kết thúc; topic UI phân biệt thiếu routine với thiếu thời gian. Chưa kiểm tra runtime.

- Game sáng Day 2 yêu cầu đã xem lời rủ trước khi bắt đầu; tương tự sau 18:00 với lời rủ tối. Người chơi vẫn có thể đổi ý sau khi xem lựa chọn. Checkpoint `choice-morning` cần kiểm tra thủ công.

- Vị trí trạm xe đi được lưu riêng với sảnh KTX và dùng lại marker trạm khi tiếp tục sandbox. Di chuyển khỏi trạm cập nhật về sảnh; nhánh Day 113 vẫn ghi đến trạm. Cần kiểm tra thủ công với `first-bus-time`.

- Thời gian chơi với Long từ 12:00 đến trước 18:00 được ghi là buổi chiều, không ghi nhầm thành lựa chọn chơi buổi tối. Fixture `choice-afternoon` dùng để kiểm tra lời rủ tối vẫn xuất hiện; chưa kiểm tra runtime.

- Lưu riêng bước vào/rời lớp và ID buổi; tiếp tục sandbox khi đứng trong lớp khôi phục backdrop, lớp và buổi đang mở. Pose ngồi và thời điểm hoàn thành 15 phút học cũng được lưu/khôi phục. Câu hỏi lớp đang mở lưu timer theo autosave; thao tác sai/thử lại lưu trạng thái ngay; checkpoint `classroom-before-seat`, `classroom-seated` và `classroom-question` chưa được manual.

- Cảnh báo lịch chỉ hiện nút thoát/chơi tiếp khi game đang chạy; lúc sinh hoạt hoặc ở trạm chỉ hiện Đã hiểu. Cần duyệt UI bằng checkpoint `first-bus-time` và `reminder-30`.

- Hoạt động ôn/bài tập tại bàn lưu môn, ghế và thời điểm kết thúc; tiếp tục sandbox dựng pose và thời gian còn lại. Hủy/hoàn thành xóa hoạt động qua transaction. Timer câu hỏi ôn/bài tập khi đang mở chưa được khôi phục; `study-in-progress` chưa được manual.

## Save và tương thích

Dùng `year-one-v3.json` + `.bak` qua cùng `YearOneCheckpointStore`/transaction. Không ghi đè hoặc migrate ngầm save v1/v2 vì lịch và academic model đã đổi. Startup cho biết cần New Game khi chỉ có save cũ. Snapshot Năm 2 giữ results/reasons, credits, debt, history, topic/midterm state, thời gian với Long và giường đã chọn.

F8 vẫn khởi động phiên mới theo yêu cầu người dùng, dùng thư mục sandbox riêng từng lượt dưới `Application.temporaryCachePath/Room404-ManualCheckpoints`. Các fixture mới có giường trên/dưới, routine, hai choice Day 2, hai mức prep và bài thi giữa kỳ dở. Xem `MANUAL_CHECKPOINTS.md`.

## Asset đã tái sử dụng

- Catalog `RemainingAssetCatalog`: `dorm_reception_background`, `npc_dorm_manager`, bộ casual/shirt/backpack front/back/side có sẵn.
- Bunk bed, bed foreground, lying/sleep và enter/exit/climb frames; bàn, ghế, book stacks, backpack hook, WC/door/wash progress.
- Backdrop lớp, phòng máy, bus và toàn bộ pose bàn học/gõ máy hiện có.
- Feast/pot frames, nhóm bạn và scene props hiện có. UI, nhãn và progress được dựng bằng uGUI/TMP.
- Âm báo thức được dựng procedural theo cách `DormSound` hiện có. Không generate raster mới và không đổi provenance của các sheet cũ.

## Đối chiếu source theo yêu cầu

Bảng này xác định nơi triển khai để rà code; **không phải kết quả chạy thử**. Các ô DoD của task vẫn chưa nghiệm thu.

| Yêu cầu | Bằng chứng source và reference đã đọc | Giới hạn |
|---|---|---|
| 12 mốc/ngày/giờ | `YearOneCalendar.Milestones`, `Sessions`, `YearOneSchedule.Courses`; graph task | Chưa chơi xuyên 12 mốc |
| Nhận phòng/giường | `YearOneArrival.CompleteArrival`, `SetupArrival`; `SelectBed`, `CommitBed`, `BedInteractable.ConfigureLevel` | Cần duyệt bố cục quầy và contact giường |
| Routine/đi học/về | `CompleteRoutine`, `SetupDailyRoutine`, WC `WashRoutine`, `TryStairs`, `CommitTrip`, `AttendSession` | Cần kiểm tra pose, route, sách/balo và timing thực tế |
| Hai choice Day 2 | `MaybeInviteLong`, `RecordInvitation`, reminders 30/15/5; preview/commit ghi skipped | Cần thử đi học/trễ/vắng và ngủ sớm |
| Kiến thức/ôn/recovery | `LearnTopic`, `RecordWeakTopic`, `CompleteTopic`, `CanStartTopic`, `BeginComputerMakeup`, `CompleteMakeup` | Cần thử đúng/sai/hết giờ, hai buổi học bù và giới hạn ngày |
| Giữa kỳ | `StartExam`, `CurrentExamExercise`, `SubmitExam`, `YearOneMidtermResult` | Cần so sánh đề theo prep và strong/neutral/weak |
| Dashboard/deadline | `YearOneCampaignUI.Refresh`, `ComputerDesktopUI.RefreshSchedule`, topic UI; `Deadline`, `AdvanceTo` | Cần đọc UI ở kích thước màn hình thật |
| Finals/credits | `YearOneExamState`, controller `Update`, `AnswerExam`/`SubmitExam`, `Eligible`, `Credits` | Chưa xác minh timer/confirm/reload/submit trong Unity |
| Bus/Long/đèn | `CampusCommuteController`, `MarkMissedBus`, `CompanionGaming`, `TurnOffCampaignLights`, restore phase | Cần thử chuyến sau, dừng game và load trước/sau tắt đèn |
| Lẩu/snapshot | `CompleteFeastStep`, `PrepareFeast`, `BeginFeast`, `FinishFeast`, `BuildYearTwoSnapshot` | Cần duyệt các kết quả tín chỉ và chuyển cuối năm |
| Skip/save lỗi | `PreviewTransition`, `CommitTransition`, controller `Transact`, store `Save`/`Load`; `SleepTransition` | Cần kiểm tra I/O lỗi, hủy và backup thực tế |
| Reset/F8 sandbox | `RestoreCampaign`, cancel trip/journey/bed, `ResetPresentation`; checkpoint window restart và LastSandbox | Chưa chạy checkpoint; fixtures thi đã có outbound history |
| Asset/tích hợp | Scene `routineCatalog` GUID trỏ `RemainingAssetCatalog.asset`; catalog có reception/manager; scripts mới có `.meta` | Không sinh ảnh mới; reference nguồn không chứng minh visual đúng |

Timer hoạt động lớp/ôn/bài tập dừng khi `SaveError` còn tồn tại, đồng bộ với việc world clock bị dừng để không hoàn tất hoạt động bằng thời gian miễn phí trong lỗi lưu.

## Nghiệm thu còn lại

Toàn bộ đường chơi và nhánh F8 cần người dùng kiểm tra. Đặc biệt: bố cục quầy/camera/collider, vị trí station gần giường/bàn, contact tầng dưới/leo thang, chuyển outfit/balo, UI topic và đề dài, save/load giữa thi, lỗi lưu, tuyến xe sau, các bước chuẩn bị lẩu và snapshot cuối năm. Không dùng source review để tuyên bố các phần này đã chạy đúng.
