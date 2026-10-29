# Long — hội thoại Năm 1, bản viết thử v0.1

Lời thoại gốc đề xuất, chưa được duyệt giọng nhân vật hoặc playtest. Các ID gắn event ở `10`, `12`, `13`; cost/effect dùng event definition, không cộng thêm từ lời thoại. Nếu actor vắng, không hiện đoạn của actor. Lươn được viết là lựa chọn ngắn để player tự hình dung giọng của mình.

## L01 — Gặp tại 404 (`EV_Y1_MEET_LONG`)

Long: “À, cậu cũng ở phòng này à? Tớ là Long. Đồ tớ hơi bừa, chưa kịp xếp.”

- **Chào, mình là Lươn.** → Long: “May quá, tớ cứ tưởng sẽ ở một mình. Cậu chọn chỗ chưa?” Chạy `greet` 5 phút; commit mới đặt met_long và memory.
- **Để mình xếp đồ đã.** → Long: “Ừ, cứ tự nhiên nhé.” `later`, chưa đánh dấu đã gặp qua hội thoại; event có thể xuất hiện lại khi Long rảnh.

Nếu chưa có thỏa thuận giường, chuyển sang `EV_BED_AGREEMENT`; không tự gán giường.

## L02 — Bữa đầu (`EV_Y1_FIRST_MEAL`)

Long: “Tớ định xuống căn tin. Cậu đi cùng không? Tớ cũng chưa biết món nào ổn.”

- **Đi cùng.** → Long: “Đi, mình xem trước rồi quyết.” Chỉ nhận lời; memory bữa chung sau khi ăn hoàn tất và cả hai có mặt.
- **Mình dọn đồ trước.** → Long: “Ừ, tớ đi trước nhé. Không gấp đâu.” Không trừ trust.
- **Mình xuống sau.** → Long: “Được. Gặp dưới đó thì gọi tớ.” Không bảo đảm Long còn ở đó.

Nếu ngân sách thiếu và hỗ trợ đã được author, UI hiện phương án hỗ trợ cùng giá; Long không tự trả tiền thay.

## L03 — Trước giờ học (`EV_Y1_LONG_INVITE`)

Long: “Xe 7 giờ 40. Làm một trận ngắn không? Tớ hứa là trận ngắn thật.”

- **Một trận.** → Long: “Được, hết trận là nhìn đồng hồ ngay.” Activity 15 phút; completion tạo memory/trust theo `10`.
- **Mình đi chuẩn bị.** → Long: “Ừ, cậu đi trước. Tớ dọn máy đã.” Không trừ trust.
- **Để lúc khác.** → Long: “Nhớ đấy nhé.” Không sinh memory đã chơi.

Sau một lượt, `EV_Y1_LONG_REPLAN` hiển thị giờ và thời gian đi trạm trước khi hỏi tiếp. Lời thoại không thay số liệu UI.

## L04 — Sau buổi sáng (`CB_LONG_EVENING`)

Nếu có `memory_long_game_d2`: Long: “Trận sáng nay, pha cuối cậu xử lý nhanh ghê.” Nếu không có: Long: “Hôm nay cậu đi học thế nào?”

Nếu player chọn **Kể mình đến muộn** và attendance là late, ghi `long_knows_late` khi dialogue commit: Long: “Tớ cũng ham trận sáng quá. Mai mình nhìn giờ sớm hơn nhé?” Long không nhận trách nhiệm thay player và không biết nguyên nhân xe nếu chưa kể.

Nếu player chọn **Không kể**, Long: “Ừ, nghỉ một chút rồi tính tiếp.” Không đổi `long_knows_late`; không tự nhận attendance là đúng giờ.

## L06 — Ôn giữa kỳ

Long có mặt khi panel `EV_Y1_MIDTERM_PREP` mở: “Tớ đang lo phần của tớ. Cậu định ôn từ đâu?”

- **Phần mình còn yếu.** → “Nghe hợp lý. Nếu bí, thử hỏi ở giờ tư vấn xem.” Mở panel topic; không tự cho Long biết điểm cụ thể.
- **Mình cần làm bài trước.** → “Ừ, xem hạn nộp trước đã.” Mở assignment record; không bắt đầu activity.
- **Mình nghỉ chút.** → “Tớ cũng thế. Nghỉ rồi tính.” Chỉ bắt đầu nghỉ khi player xác nhận activity.

Nếu Long không available, panel học vẫn mở, không có lời thoại thay thế của Long.

## L09 — Đèn khuya (`EV_Y1_ROOM_LIGHT`)

Long: “Lươn ơi, mai tớ phải dậy sớm. Mình bớt sáng đèn phòng được không?”

- **Tắt đèn phòng, dùng đèn bàn.** → “Cảm ơn. Ánh đèn bàn quay phía cậu chắc tớ ngủ được.” Chạy choice `desk_lamp` nếu có nguồn.
- **Cho mình thêm 30 phút?** → “Để tớ xem mai phải dậy lúc nào.” Nếu lịch cho phép: “Được, 10 giờ 35 nhé.” Nếu không: “Tớ cần ngủ ngay. Mình thử đèn bàn được không?”
- **Mình vẫn cần đèn phòng.** → “Ừ… nhưng tớ khó ngủ với ánh sáng này.” Chạy `keep_light`, conflict mở theo `12`.
- **Mình dừng, tắt đèn.** → “Cảm ơn nhé. Bài mai nếu cần thì mình tính tiếp.” Chạy `stop_work`.

## L10–L11 — Nói chuyện sau đó (`EV_Y1_LIGHT_FOLLOWUP`)

Nếu conflict còn mở, Long: “Tối qua tớ ngủ không ngon. Tớ nên nói sớm hơn, nhưng mình thống nhất giờ tắt đèn được không?”

- **Xin lỗi, mình thống nhất giờ nhé.** → nếu Long chấp nhận theo predicate: “Ừ. Có hôm cần học khuya thì nói trước với tớ.” Resolve sau 5 phút và commit.
- **Để tối mình nói tiếp.** → “Được, miễn là mình đừng quên.” Conflict vẫn mở.
- **Mình không muốn bàn.** → “Ừ, tớ sẽ để cậu yên. Nhưng chuyện đèn vẫn còn đấy.” Conflict vẫn mở, không tự hạ thêm trust.

Nếu không có conflict: Long: “Hôm qua tớ ngủ được rồi, cảm ơn nhé.” Không cộng quan hệ chỉ vì hiện thoại.

## L15 — Bữa lẩu cuối năm (`EV_Y1_HOTPOT`)

Chỉ nếu Long thực sự tham dự và còn có mặt tại thời điểm thoại. Chọn dòng đầu tiên có predicate đúng, không gộp tất cả; predicate memory chỉ xét memory mà Long biết. Nếu dòng khác ưu tiên hơn, callback slide bị bỏ qua trong bữa này, không xếp hàng phát sau:

1. `conflict_light=open`: “Ăn xong mình nói chuyện giờ đèn nhé? Tớ không muốn để chuyện nhỏ kéo dài.” Không tự resolve.
2. `MEM_LONG_SLIDES_D22` tồn tại, outcome `helped`, Long thuộc `knownBy`: “Hôm cậu giúp tớ xếp slide, tớ bớt run hẳn. Cảm ơn cậu nhé.”
3. `first_meal_long`: “Bữa đầu ở căn tin, tụi mình còn đứng nhìn menu mãi. Giờ ít nhất cũng biết chọn đồ nấu.”
4. `memory_long_game_d2`: “Ăn xong đừng rủ chơi trước giờ học nữa nhé. À, mai đâu có tiết.” Không tự nói player đã lỡ xe.
5. Không có memory hợp lệ: “Năm nay phòng mình cũng có nhiều chuyện. Ăn đi, nước lẩu sôi rồi.”

## Chưa đạt

Các đoạn trên là bản viết cho 7 cụm event, không phải 15 scene hoàn chỉnh, không có voice/localization/UI runtime. L06 và các cửa sổ availability còn thiếu event record chi tiết. Cần kiểm tra callback theo NPC knowledge, Long vắng, memory rỗng, lựa chọn không tương tác và save/retry; chưa chạy hoặc playtest.
