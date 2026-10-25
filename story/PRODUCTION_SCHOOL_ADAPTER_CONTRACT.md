# Hợp đồng nối M06–M08 vào production

Nguồn: PRODUCTION_FLOW_BASELINE.md, production `missions.json`, và các controller Unity hiện có.
M06–M08 đã nối gameplay vào scene Production_Opening và qua fullM01–M08/739checks. Còn art attachments/audio/motion/device; tham khảo PRODUCTION_YEAR_ONE_REVIEW.md cho scope và bằng chứng hiện tại.

## Quyền sở hữu bắt buộc

- `ProductionCampaignSession` là nguồn ví, inventory, mission và tín chỉ duy nhất của tuyến production. Không mở lại New Game năm nhất hoặc sao chép ví 110.000đ từ `YearOneProgress` khi rời M05.
- `DormJourneyController` giữ vai trò di chuyển/WC/fade và khóa actor. `CommitWashCompletion` được gọi sau wash trong lúc còn khóa actor; nếu lưu thất bại giữ `HasRefreshed=false`, trả quyền để thử lại. Không polling rồi ghi lặp ở mỗi frame. `ConfigureRoomView` cho phép chỉ định camera phòng khi scene bắt đầu tại bến xe.
- `CampusCommuteController` dùng lại chuyển động xe, lên xe, giấu/khôi phục renderer và âm thanh. `ICommuteProgress`/`ProductionCommuteProgress` đã tách nguồn ví và commit khỏi `YearOneCampaignController`. Binding production từ chối đồng thời owner legacy; chỉ cho M06 take_bus/return_room và M07 catch_bus đúng choice.
- Chỉ một controller giữ token transition của player. Sự kiện thành công và checkpoint phải commit trước khi trả quyền ở điểm đến; lỗi save giữ mốc an toàn trước chuyến, không vừa charge hai nguồn ví.
- Không dùng nguyên `YearOneMilestoneController.SolvedPuzzle()` ở nhánh không có campaign: nhánh đó mở thẻ 32 tín chỉ sau một câu đố. Production chỉ được nhận 32 ở kết quả M07 theo baseline.
- Không gọi `MVPFlowController.CompleteExtendedYearOne()` khi hết M08: nó mở gaming stage demo. Production phải mở đăng ký M09 trước.

## M06

| Sự kiện | Trình bày/thao tác phải chứng minh |
|---|---|
| equip_backpack | Lấy đúng balo trên giường đã chọn; đồng bộ prop/visual bằng flag production |
| wash_before_class | Ra hành lang, dùng WC Nam; callback sau wash hoàn tất mới ghi event |
| take_bus | Đi tới trạm, đợi xe dừng, lên/khởi hành; một vé 7.000đ từ ví production |
| enter_class | Đi từ cổng H6 tới lớp, marker/camera/foreground đúng |
| solve_warmup | Tái dùng CalculusPuzzleModal: tổng 1..10=55, sai/timeout retry, không tự pass |
| finish_lesson | Kết thúc tiết học, đứng lên và trả khóa presentation; không cấp tín chỉ năm |
| return_room | Xe về KTX, một vé riêng; lên phòng qua đường đi hợp lệ |
| tell_dam | Kể cho Đạm tại404; bạn H6 dùng C00 classmate, không đổi lại thành Đạm |

Tái dùng YearOneSchedule/Exercise và minigame question; không kéo toàn bộ state machine demo sang để vô tình chạy hai quest graph.

## M07

- Card/ngữ cảnh báo một buổi sáng khác 07:20 trước khi chuyển tới queue WC; reset daily flags, giữ đồ và ví.
- Đợi lượt, đánh răng/rửa mặt, ra trạm. Đúng giờ hoặc lỡ chuyến đều có lối đi thật.
- Fallback phải có xe chuyến sau/điểm đón phụ, không teleport đi bộ miễn phí tới H6. Vé vẫn 7.000đ, event choice `next_bus` khác `on_time`.
- Vào lớp/điểm danh có phản ứng phù hợp. Lựa chọn muộn không biến thành bị ép trượt input.
- Chuỗi ngày đại diện và thi phải được trình bày trước thẻ 32 tín chỉ. Nếu thu gọn thành montage có tương tác, phải có bảng thu/chi và kết quả môn; không dùng một nút “hoàn thành năm”.
- Ledger đã chốt: M06hai lượt + M07một lượt=21k; `review_commute_ledger` trích quỹ9lượt còn lại63k. Course/summary/chuyến về dùng quỹ đã reserve, không trừ lại; không ghi ví legacy đè session production.

## M08

- 32 tín chỉ/không nợ đã xác nhận mới mở chuẩn bị lẩu.
- Lấy bếp/nồi, cắm điện, thêm nguyên liệu, ăn, cụng ly là các event riêng; dùng R_POT/R_FLOOR/R_SWITCH và props có sẵn.
- Pose sàn không dùng bộ pose ghế C02. Khi nhóm thay player presentation, chụp/khôi phục renderer riêng; không bật mọi renderer mù quáng.
- Ngủ/chuyển kỳ mở M09 đăng ký. Không nhảy gaming, năm5 hoặc A tốt nghiệp.

## Bằng chứng hiện có và giới hạn

Campaign năm nhất cũ đã có WC/12 tiết/6 ngày/thi/lẩu và các kiểm tra Play Mode. Lượt Play Mode kết thúc 25/10/2026 lúc12:55UTC đạt514 checks/0errors, xác nhận sửa chuyến sau, fare và checkpoint. Data/presentation checks mới đạt117 assertions; host production đạt91 assertions, gồm save thất bại không trừ vé. Objective recovery hiện chờ/lên chuyến sau và bỏ đồng hồ hết giờ. Kết quả của campaign cũ chỉ chứng minh thành phần có thể tái dùng; không thay cho playthrough production M01→M09 và không chứng minh toàn45mission.

## Hợp đồng xe sau khi tách host

- `BindProgress` chỉ thực hiện khi xe không giữ transition; `MoneyVnd` đọc trực tiếp ví session khi đã bind, không trừ thêm ví local.
- `TryBoard` yêu cầu điểm đến (`CampaignArrival`), bus dừng, balo/WC và đúng mission/step/direction/7k. Đang lên xe khóa input; double input không tạo chuyến thứ hai.
- Xe chạy xong, giữ fade/lock và commit destination vào checkpoint trước callback trình bày điểm đến. Chỉ tăng ticket count sau commit thành công.
- Nếu commit thất bại: chưa trừ tiền, không gọi callback điểm đến, trả actor về trạm cũ, restore renderer/physics/control và cho lên lại xe đã đỗ.
- Checkpoint đã commit là destination ổn định (H6/Room); reload không chạy lại charge. Callback điểm đến phải restore actor/đặt spawn/camera rồi mới trả transition. Với `return_room` phải trình bày tới phòng theo hợp đồng mission, không trả quyền tại cổng và giả vờ đã về404.
- Host stale sau commit từ chối mọi lần gọi tiếp; callback không được gọi API tự cấp32 tín chỉ hoặc mở gaming demo.

Bằng chứng máy đọc: `assets/mvp_sprites/remaining/review/production-commute-host.json`. Fixture Play Mode xe/WC được báo riêng trong `review/commute-playmode/`, không ghi đè271checks của opening và không tuyên bố routeM06–08 hoàn tất.

Fixture kết thúc13:10:39UTC: **59 checks,0errors**. Bao gồm xe tới thật, lên/khởi hành, lỗi lưu trả về trạm, thử lại thu đúng một vé, reload, xe chuyến sau, ngắt boarding, callback WC còn khóa actor và camera phòng được cấu hình trướcAwake. Source/report hash được lưu tại `review/production-school-adapter-evidence.json`.

Cập nhật13:40UTC: M06 đã có full opening proof390checks/0errors. `review/opening-runtime-evidence.json`149hash thay cho việc dùng fixture cũ để suy ra route. M07–M08 vẫn theo hợp đồng pending ở trên; không dùng kết quảM06để đánh hoàn tất năm1.

Bằng chứng hiện tại: full739checks/0errors, aux438checks(both travel choices/expired parked bus), timer/curriculum42assertions, lifecycle52checks. Model45mission/212event; nghiệm thu art WC/lẩu4attachment còn pending. Xem review M07–M08mới; các đoạn390/149ở trên là mốc lịch sử củaM06.
