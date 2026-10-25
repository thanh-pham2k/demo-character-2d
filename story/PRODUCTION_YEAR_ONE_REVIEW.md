# Production M07–M08 — buổi sáng khác, năm học và lẩu

Ngày triển khai:25/10/2026. Scene:`mobile-2d/Assets/Scenes/Production_Opening.unity`.
Trạng thái: gameplay prototype M01–M08qua739checks/0errors, kết thúc2026-10-25T14:43:28.1057670Z. Hai giường/hai kết quả xe; chưa nghiệm thu toàn bộ art detail/mobile/build/full game.

## M07: lựa chọn chuyến xe có hậu quả và lối đi đầy đủ

- Sau phản ứng của Đạm ởM06, một sáng khác bắt đầu tại404lúc07:20. Balo giữ nguyên, flag vệ sinh của buổi trước được reset cho buổi này.
- Đồng hồ mặc định120giây gameplay, dừng trong menu/background pause. Timer có checkpoint riêng, gắn definition hash và lịch sửM01–M06; tải lại không bắt đầu lại120giây. Dữ liệu hỏng được giữ, backup hợp lệ có thể phục hồi. Không trộn timer với ví/tín chỉ.
- Ra hành lang, đứng cuối hàng, chờ8giây và từng NPC nhường lượt; đợi xong vẫn phải vàoWC Nam/rửa mặt thật mới ghi event. Chưa tới lượt thì không thể dùng WC hoặc xuống cầu thang. Reload trước event wash lặp lại phần queue chưa commit và giữ thời gian đã trôi.
- Đi tới trạm33. Khi còn giờ, xe đến và có thể lên. Khi đã lỡ, xe chuyến sau phải ghé thật. Nếu xe cũ đã đỗ mà người chơi đợi tới quá giờ, xe cũ rời trạm trước, khóa boarding rồi mới gọi chuyến sau.
- Chỉ charge7.000đmột lần qua session production ở điểm đến. `on_time`/`next_bus` được lưu đúng theo đồng hồ; thầy điểm danh có phản ứng riêng, đều hội tụ vào lớp học.

## Chuỗi học/thi và sổ cái

M07có18event, thay cho5event tổng quát trước đây. Toàn campaign vẫn45mission nhưng hiện212event thành công có thứ tự. Dữ liệu nguồn:`Resources/ProductionCampaign/year-one-courses.json`; đây là chương trình cân bằng hư cấu trong game.

| Học kỳ / Chặng | Học phần | Tín chỉ | Lũy kế sau thi |
|---|---|---:|---:|
| 1 / 1 | Giải Tích1 | 6 | 6 |
| 1 / 2 | Đại số tuyến tính | 5 | 11 |
| 1 / 3 | Nhập môn Tin | 5 | 16 |
| 2 / 4 | Cơ học đại cương | 5 | 21 |
| 2 / 5 | Tiếng Anh1 | 5 | 26 |
| 2 / 6 | Kỹ năng học đại học | 6 | 32 |

Mỗi môn có bài học và đề thi khác nhau. Sai/timeout có retry, Esc bỏ dở không ghi pass. Bài học không cấp tín chỉ; pass đề thi mới commit môn/tín chỉ lũy kế. Save/load giữa năm giữ môn đã pass, không thi lại hoặc cộng credits lần nữa. Thẻ đổi học kỳ làm rõ montage thời gian. GT2/VL1/KTLT của arc rớt môn năm2không bị đánh dấu đã pass bởi các môn nhập môn này.

Sổ vé giải thích6ngày đại diện/12lượt: M06đã trả2lượt, M07trả1lượt; trích quỹ63.000đcho9lượt còn lại. Tổng84.000đcho12lượt đi học, chưa tính vé nhập họcM01. Cảnh montage và chuyến về sau kết quả dùng quỹ này, không charge thêm lần nữa. Ledger và kết quả từng môn phải hoàn tất trước tổng kết32/150/không nợ.

## M08: hành động riêng, presentation riêng

1. Đọc kết quả vềKTX/phòng rồi mới mở lẩu. Lấy bếp/nồi ở góc; đặt quạt sang bên để chừa chỗ.
2. Đi sang đầu dây bên phải, giữ Space hoặc nút touch1giây để cắm điện. Partial hold/cancel chưa nối điện; dây chỉ bật khi event đã lưu.
3. Thêm nguyên liệu do các bạn góp. Không trừ thêm một giá bữa ăn chưa được author trong ledger.
4. Cùng ăn và nghe thoại nhóm. Dùng4pose ngồi sàn đang có; ẩn đúng renderer của player và các bạn đứng, khôi phục khi cancel/đóng. Không dùng pose ghếC02để ngồi sàn.
5. Cụng ly là bước riêng sau ăn; không tự mở năm2chỉ vì đã có32tín chỉ.
6. Tới đúng giường đã chọn, ngủ/chuyển kỳ3; mởM09`refresh_registration`, không gọi nhánh gaming demo và chưaFinished.

## Tương thích save và tài sản

Definition trước được archive theoSHA và chỉ chấp nhận prefix tới hếtM06. Lịch sửM07cũ không được diễn giải thành môn/tín chỉ/ledger mới. Tiền từM05/M06giữ nguyên; chỉ event ledger mới reserve63k. Nguồn crop/sheet/PNG của24batch/178outputs không thay đổi.

Tái dùng C00NPC làm người chờ, bus33/campus/H6/WC, study pose, pot0/1,4floor poses và ổ điện. Dây dựng bằngLineRenderer/Unity material; UI/text/thẻ học kỳ dựng bằngUnity. Không gọi imagegen hoặc slice thêm.

## Bằng chứng và giới hạn

- Model:`review/production-campaign-model.json`369.301assertions/192tổ hợp/212event; gồm ví, course graph, mọi mốc tín chỉ và compatibility prefixes.
- Timer/curriculum:`review/production-year-one-data.json`42assertions, gồm pause/reload,0giây giữ trạng thái missed, backup/invalid-file preservation.
- Route riêng:`review/year-one-production-playmode/report.json`438checks, gồm on-time/next-bus,6môn, save/load giữa năm, lẩu/sleep, và xe đã đỗ rồi hết giờ.
- Fullroute739checks,0errors; source/scene/data/captures261hash trong `review/opening-runtime-evidence.json`. Fixture lifecycle52checksđộc lập xác nhận update/disable khi đã hủy actor. Timer chỉ được rút còn3giây trong nhánh test lỡ xe và20giây trong fixture xe đỗ; scene tác giả giữ120giây.
- Đây là prototype gameplay, chưa phải full45mission. M09–M45, audio/motion nghệ thuật, touch/device/build/full-ending gates vẫn pending. Pose lẩu/cụng ly chủ yếu là pose sàn cũ; không coi là animation contact/mug đã nghiệm thu.

### Lỗi lifecycle tìm thấy trong lượt full đầu

Lượtfullđầu đã chạy tớiM09trên cả hai nhánh nhưng bị đánh FAIL vì `OnDisable` truy cập một floor actor đã bịUnityhủy khi dỡ scene. Đã sửa cleanup theoUnity-null cho feast/study/dinner snapshot và bổ sung tình huống cố tình hủy actor trước khi disable controller. Lượtfullđang chạy lại; không bỏ qua exception để lấy báo cáo xanh.

### Bản cuối sau sửa lifecycle

FullM01–M08đã đạt739checks/0errors. Cleanup kiểm traUnity-null cho actor/renderer; update queue/feast cũng bỏ qua actor đã bị hủy. Trước lượtfullcuối, fixture52checksđã chủ động hủy floor/queue actor khi controller còn chạy rồi disable. Snapshot261file hiện là bằng chứng chấp nhận prototype. Hai lỗi trước được giữ trong lịch sử review, không dùng reportFAILđể đánh pass.

### Art detail chưa nghiệm thu

Bốn asset đã có nhưng chưa bind vào runtime này: `toothbrush_cup`, `towel_folded`, `dried_buffalo_meat`, `drinking_glass`. WC dùng cửa/wash/progress; các floor poses đã bake ly. Cần gắn props còn thiếu hoặc chốt reuse có lý do trong pass art tiếp theo. P00meat/vegetables hiện tái dùng làm nguyên liệu bữa lẩu; không ghi các attachment chưa có là đã tích hợp. Không cần generate ảnh mới.
