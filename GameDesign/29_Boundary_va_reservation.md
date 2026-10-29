# Quy tắc boundary, energy và reservation — v0.1

Đề xuất bổ sung cho `02`, `04`, `10`, `12`, `13`; chưa có code, chưa kiểm chứng. Các trường và transaction này là specification chứ không phải API đã tồn tại.

## Chọn xe và mốc đóng cửa (C02)

`SelectedTrip`: tripId, departure, origin, destination, status `selected|boarded|missed|cancelled`. Từ lịch, player chọn/chuyển chuyến đã biết, 0 phút, không mua vé, chỉ một selection active cho tuyến; đổi chuyến trước khi chuyến cũ đóng ghi `cancelled`, không ghi missed. Không chọn vẫn được đến trạm và tự board khi có xe; chỉ tạo cờ missed nếu selection active khi chuyến đóng mà chưa board.

`boardingDecision`: tripId, timestamp, state `offered|boarded|declined|invalid`, decisionId. Khi đồng hồ đạt departure, scheduler hoàn tất travel đến trạm trước; nếu player ở đúng trạm và xe/cửa sẵn sàng, dừng world clock, mở decision một lần và lưu trạng thái. `board`: kiểm tra tiền còn khả dụng, reserve và commit vé + boarded cùng transaction; `not_board`: ghi declined rồi đóng cửa; `cancel selection`: cancel chọn chuyến và đóng cửa, không missed. Không dùng timeout thời gian thực; nếu player đóng panel, xử lý như `not_board` sau xác nhận rõ. UI không cho bắt đầu travel khác khi decision đang pending.

Sau khi decision resolve, scheduler đóng cửa và nếu selection vẫn active chưa boarded ghi missed một lần. Nếu player đang ở nơi khác tại departure không mở decision, đóng ngay và ghi missed nếu selected. Selection không có không tạo missed; nếu không còn xe thì UI ghi no_bus_available. Một trip không tự giữ chỗ hay bảo đảm còn capacity trừ khi capacity được author.

Save/load pending decision khôi phục đúng choice và clock, không đóng cửa tự động; tiền chưa commit không mất. Save lỗi khi boarding giữ decision pending, xe chưa rời, không trừ tiền. Input hai lần cùng decisionId chỉ commit một lần. Ví dụ fixture D2: tới 07:40 từ travel kết thúc đúng mốc, decision mở; board → trừ 3M, đóng cửa, tới H6 07:45; not_board → đóng cửa và missed nếu selected; không selection + not_board → không ghi missed.

## Tiêu hao energy khi gián đoạn (C04)

Mỗi activity có `durationMinutes > 0`, `energyCostTotal >= 0`, `energyApplied`, `elapsedMinutes`, `milestones`. Tại elapsed nguyên `t`, cumulative cost `floor(energyCostTotal * t / durationMinutes)`; tại completion áp dụng đúng total. Mỗi tick/milestone chỉ trừ chênh lệch cumulative so với `energyApplied`, không round từng lần riêng. Save/load, pause/resume hoặc chia tick không thay đổi total. E clamp 0–100; recovery là một effect riêng được commit tại milestone quy định, không tính như negative cost.

Ví dụ 45 phút E −7: ở phút 20 đã tiêu floor(7×20/45)=3; tiếp tới phút 45 chỉ tiêu thêm 4. Activity nhiều milestone định nghĩa tổng duration/cost theo session, hoặc mỗi milestone là một activityRun có ID riêng; không tạo run mới chỉ vì pause. Fixture assignment có hai run 45 phút E −7; nếu đã hoàn tất run đầu và dừng ở phút 20 run thứ hai, tổng tiêu 10 E, không cấp milestone thứ hai.

Trước bắt đầu, UI hiển thị total, điều kiện đủ E và điểm gián đoạn. Activity interruptible dừng tại phút đầu tiên E về 0, giữ progress nếu definition cho phép; activity không gián đoạn chỉ được start nếu có đủ E. Exam được course định nghĩa ngoại lệ đã công bố, không âm E. Chưa quyết định tác động mệt tới hiệu suất; không tự áp penalty.

## Tiền và reservation cho bữa ăn (C07)

`MoneyReservation`: id, activityRunId, amount, status `held|captured|released`; `available=balance−sum(held)`. `A_MEAL_LONG` reserve 8M tại start; phải có đủ available, nếu không thì hiển thị phương án hỗ trợ đã author. Khi hoàn thành, atomically capture reservation (balance −8) cùng effect +5 E/memory/trust; nếu ngắt hoặc hủy trước completion, release khoản held, không mất tiền nhưng thời gian/E đã tiêu vẫn giữ. Không thể tiêu 8M đang hold cho activity khác; không áp dụng lặp capture khi load.

Save lỗi lúc start không giữ reservation; save lỗi lúc completion giữ snapshot trước completion và reservation held, không apply memory/điểm/thu tiền. Resume có thể retry hoặc cancel để release. Nếu Long rời trước khi activity bắt đầu, không start và không reserve. Nếu Long rời giữa activity, event phải định nghĩa interruption và actor có mặt; không mặc định ghi memory bữa chung. Bữa miễn phí reserve 0M; không giả định người khác trả tiền.

## Giường khi Long tới trước (C08)

Hai giường `free|assigned_player|assigned_long`; player có quyền chọn cả hai khi Long chưa commit nhận giường. Nếu Long tới trước, `EV_BED_AGREEMENT` cho Long đề nghị một giường nhưng chưa assign: player chọn giường còn lại, đề nghị đổi hoặc để sau. `propose_swap` mở dialog, Long có thể đồng ý theo event; chỉ khi hai bên đồng ý mới commit hai assignments atomically. Nếu Long từ chối, player có thể chọn giường còn lại, hoãn hoặc tiếp tục trao đổi theo event; không đổi giường đã assigned mà không có consent. Nếu player chưa chọn, không tự gán dựa trên việc NPC xuất hiện.

Sau agreement, sleep cần player có assigned bed; hai state không được cùng assigned một giường. Một trong hai có thể tiếp tục các hoạt động khác khi chưa thỏa thuận; trước giờ ngủ UI nêu rõ cần chọn chỗ. Các điều kiện Long đồng ý/từ chối và thời gian dialog cần author; không tuyên bố đã cân bằng.

## Đối chiếu cần thực hiện

Case arrival-before/exact/after departure, selected/cancelled/no selection, save lỗi từng bước, split ticks 45 phút, gián đoạn giữa hai milestones, hai purchase cạnh tranh reservation, Long rời lúc ăn, hoãn/đổi giường. Đây là expected cases, chưa chạy simulation hoặc playtest. Các calendar, retake, job recovery và program graph ở C01/C03/C05/C06 vẫn thiếu.
