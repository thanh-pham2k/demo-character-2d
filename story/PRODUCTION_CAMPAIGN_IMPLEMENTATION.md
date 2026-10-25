# Tiến độ tuyến production45 mission

Cập nhật25/10/2026. Đây là phần triển khai nền tảng của plan, chưa phải game hoàn chỉnh.
Các thay đổi nội dung và gameplay cần qua design gates trong [PLAYER_EXPERIENCE_PLAN.md](PLAYER_EXPERIENCE_PLAN.md); tài liệu này tiếp tục là nguồn sự thật cho runtime và bằng chứng triển khai.

## Đã triển khai

- `mobile-2d/Assets/Resources/ProductionCampaign/missions.json`:45 mission,212 sự kiện thành công có thứ tự; entry/bridge theo baseline. Dữ liệu được author bởi `story/plans/build_campaign_definition.py`.
- `ProductionCampaignDefinition`: kiểm tra cấu trúc/version/ID/choice/effect.
- `ProductionCampaignProgress`: chỉ nhận đúng mission/action/choice hiện tại; không có API hoàn thành mission tùy ý. Sự kiện sai/lặp không đổi ví, tín chỉ hay lịch sử.
- Inventory khởi đầu: giấy nhập học, balo, tua vít. Vé được mua/tiêu thụ; giấy nộp trước nhận khóa; khóa cần để vào404 và được trả ởM44.
- Cờ lựa chọn khác nhau cho chuyến xe, gamingYES/NO, wake/snooze, cuộc gọi mẹ; hội tụ trong graph. Phản ứng cụ thể trên scene/năng lượng vẫn cần adapter.
- Ledger dùng chung, thưởng50k duy nhất; trợ giúp chỉ bù đúng thiếu hụt cho chi phí hiện tại, có lịch sử và không thể lấy dư.
- Graph KTLT → CTDL → HĐH, GT2 đã pass được giữ; mốc32/50/78/118/136/150; đủ điều kiện mới đăng ký đề tài; đạt phản biện mới nhậnA.
- Ba bạn rời rồi mới thay bằng ba người mới; Đạm chưa bay trước tiếp tế M34. M31 chỉ báo trước việc rời đi; người mới thứ ba chỉ vào sau khi Đạm đã chuyển đi và giường trống. M44 giữ đồ của người mới, chỉ thu dọn góc Lươn/trả khóa; chỉM45 nhận bằng, khoác vai và credits mới Finished.
- Checkpoint chỉ lưu lịch sử sự kiện và hash phiên bản dữ liệu; tải bằng replay để bác dữ liệu sai thứ tự. Không serialize pose/coroutine/transition lock.
- Ghi file tạm rồi thay file, có backup; khi primary hỏng không ghi đè backup hợp lệ. Save khác definition/cả hai bản lỗi phải báo phục hồi/migrate, không âm thầm bắt đầu lại.
- `ProductionCampaignSession` ghi đĩa trước khi công bố trạng thái mới. Lỗi đĩa giữ nguyên tiến độ đang chạy; giao diện view chỉ đọc để adapter không tự sửa model của session.

## Giới hạn và cổng tiếp theo

M01–M09 đã nối mô hình vào scene riêng `Production_Opening` và qua819checks PlayMode ở hai giường/hai kết quả xe; xem [PRODUCTION_YEAR_ONE_REVIEW.md](PRODUCTION_YEAR_ONE_REVIEW.md). Lớp này **chưa thay controller trong scene Room404_MVP**. Model riêng không chứng minh minigame/timer; full739checks hiện chứng minh prototypeM01–M08 với input/di chuyển/puzzle/deadline, chưa hoàn thiện45cảnh. Các adapter chỉ được gửi sự kiện thành công sau khi thao tác thật/mini game đạt; không dựng nút “hoàn thành mission” để chạy xuyên graph.

Các số tiền trong mô hình là cân bằng prototype:35000 ban đầu,7000 vé,5000 bộ vệ sinh,20000 suất cơm,100000 hỗ trợ đầu kỳ;3 job đại diện mỗi 50000;ledger nhiều tuần 1200000 và học phí 900000. Chúng chưa thay giá25000của cơm trong runtime năm nhất hiện có. Cần thống nhất menu/budget khi nối adapter, đo playtest rồi chốt; không trình bày đây là chi phí hay học phí thật.

Thứ tự nối runtime:

1. M01–03 (đã có prototype riêng, còn polish): bến xe/sảnh/giấy/khóa/claim bed/balo/quạt; tái dùng asset đã tạo và hệ thống input/presentation hiện có.
2. M04–08: nối sự kiện cleaning/canteen/H6/trip/feast vào graph; giữ lớp kiểm tra năm nhất đã có. **M07 legacy hiện còn fallback đi bộ tới H6; phải đổi thành chuyến xe sau/điểm đón phụ theo baseline trước nghiệm thu**, có fare/recovery/save đúng.
3. M09–16: thêm đăng ký, nhánh đêm/sáng, kiểm tra lẩu, bàiC++,thi,portal/cuộc gọi. Chặn stage demo nhảy thẳngnăm 5 khi adapter production hoạt động.
4. M17–36: jobs/ledger/energy, tình cảm, coursegraph, thay phòng, phục hồi học tập.
5. M37–45: đề tài/design/code/demo/report/crash/defense, packing/key/degree/reunion/credits.
6. UI/font/touch/pause/audio, QA transition/retry/save, motion/scene QA vàbuild.

## Bằng chứng kiểm tra

`assets/mvp_sprites/remaining/review/production-campaign-model.json` là báo cáo Edit Mode của mô hình:192 tổ hợp lựa chọn,212 event mỗi tuyến, replay/checkpoint/corruption/disk failure và các invariant ở mốc. Hash definition trong report phải trùng file hiện tại. Báo cáo này không thay full Play Mode.

Bộ Play Mode 506 check trước đó chỉ bao phủ nền tảng interaction và campaign năm nhất, gồm bạn cùng lớp H6 và phản ứng của Đạm tại404. Không cộng hai kết quả để tuyên bố toàn45 missionđã chơi được.

Kết quả hiện tại: PASS 369.301 assertion trên 192 tổ hợp lựa chọn, 212 event mỗi tuyến; 0 lỗi. Definition hash trong báo cáo khớp file hiện tại.

M04–M05: đã có prototype mua dụng cụ, dọn bốn điểm/timer/retry, làm quen, menu cơm có ngân sách và ăn/về ngủ. Xem [PRODUCTION_HOUSEKEEPING_REVIEW.md](PRODUCTION_HOUSEKEEPING_REVIEW.md). Ba suất cơm cơ bản là 10.000/15.000/20.000đ, thêm trà 3.000đ; lựa chọn được lưu trong event và dùng cùng ví production. Save definition cũ chỉ được chuyển nếu hash được liệt kê và lịch sử không vượt M04; định nghĩa cũ được lưu nguyên bản.

## M07–M08 đang nghiệm thu runtime — 25/10/2026

M07 được phân rã thêm13event: ledger dự trù9vé/63k và6cặp bài học/thi với checkpoint/tín chỉ riêng. Dữ liệu `year-one-courses.json` tổng32credits, không tự passGT2/VL1/KTLT của arc năm2. Clock gameplay120s có store gắn prefixM01–M06; menu/reload không reset thời gian. M08 có hold cắm bếp, nguyên liệu góp,4floor poses, cụng ly và ngủ mởM09. Đọc `PRODUCTION_YEAR_ONE_REVIEW.md` cho giới hạn và bằng chứng riêng. LượtfullM01–M08đang kiểm tra lại cleanup scene; chưa dùng báo cáo cũ để xác nhận source mới.

Bản cuối14:43UTC: full739checks/0errors và lifecycle52checksđã qua; snapshot261file cóhash đồng bộ. M09–M45 và art attachments/audio/motion/device/build vẫn pending. LượtfullFAILlifecycle trước đã được sửa cả cleanup và update, không bỏ kiểm tra.

M09đã nối physical laptop/portal và qua full819checks(M01–M09), fixture87checks. Snapshot292hash hiện tại. Wallet/credits/definition212event giữ nguyên; M10–M45/contact/motion/audio/device/build/fullending chưa đạt. Kiểm tra tập trung cho thay đổi cục bộ, fullroute tại mốc lớn để tránh lặp tốn thời gian.
