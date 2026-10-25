# Production M06 — buổi học đầu tiên

Mốc review lịch sửM06. Bằng chứng hiện tại đã mở rộng M01–M08/739checks/261hash; xem [PRODUCTION_YEAR_ONE_REVIEW.md](PRODUCTION_YEAR_ONE_REVIEW.md).

Ngày triển khai:25/10/2026. Scene:`Assets/Scenes/Production_Opening.unity`. Phạm vi code hiện đã nối M01–M06; M07–M45 còn triển khai. Bằng chứng hiện tại: full Play Mode M01–M06 qua hai lựa chọn giường,390checks/0errors, kết thúc13:40:09UTC. Đây là nghiệm thu prototype, không phải full game hoặc mobile/build.

## Flow đã nối

1. Sau bữa cơm và ngủ M05, lấy balo đúng giường trái/phải đã chọn; prop trên giường được ẩn, visual đeo balo bật.
2. Đi tới cửa phòng → hành lang. WC Nữ từ chối; WC Nam có cửa mở/đóng, đi vào/ra, thanh tiến trình, ẩn/khôi phục renderer và callback lưu khi actor còn khóa. Chưa rửa mặt thì chưa xuống cầu thang.
3. Tới cầu thang → sảnh/trạm33. Đi bộ tới biển trạm, đợi xe chạy vào và dừng, bấmE hoặc Tương tác để lên. Ví production trừ7k đúng một lần khi commit điểm đến, trước khi trả quyền ởH6.
4. Đi tới cửaH6 → lớp. Bạn cùng lớp dùng C00 classmate, không phải Đạm. Thầy/bạn giới thiệu câu hỏi, ngồi học và trả lời tổng1..10=55. Sai phải retry, Esc đóng không ghi success. Warm-up không cấp32tín chỉ.
5. Ghi lời giải và kết thúc tiết; ra cửa lớp → trạm xe về, đợi/lên chuyến riêng và trả7k. Callback về thể hiện KTX/cầu thang/tầng4 trong transition khóa input rồi mới trả quyền trong404.
6. Tới Đạm, kể buổi học đầu; Đạm họcHCMUS, phản ứng ởKTX. Sau thoại mở buổi sáng khácM07, vẫn0tín chỉ. Không gọi quest graph demo, không mở gaming hoặc tự hoàn tất năm.

## Checkpoint và nguồn sự thật

Session production sở hữu ví/event/inventory. Điểm lưu sauequip=Room, sauwash=Hallway, saubus=H6, sauenter/puzzle/lesson=H6Classroom, sauvévề=Room. Trạng thái điều hướng chưa commit (cửa/cầu thang/đi bộ tới trạm) được dựng lại từ điểm lưu an toàn; reload không tạo vé mới.

Definition cũ được archive theoSHA; chỉ prefix kết thúcM05 được khai báo tương thích với thay đổiM06. Save cũ đã đi quá ranh giới được giữ nguyên/đọc backup hợp lệ, không tự diễn giải lại. Model kiểm tra cả hai quy tắc tương thích.

## Tái sử dụng mỹ thuật

Bus33/biển trạm, campus, hallway, WC với door leaf/interior, H6gate/classroomv2, teacher, study pose và desk foreground đều dùng file đang có. Không generate/slice thêm. C00 classmate được bind trực tiếp trong scene production. UI câu hỏi/text/clock/progress dựng bằngUnity, font Việt dùng chung.

## Giới hạn cần giữ rõ

- Chuyến về hiện bao gồm đoạn lên tầng4 bằng transition có trình bày vùng, chưa phải đoạn cầu thang điều khiển tự do.
- Đã xem capture hành lang/WC/trạm xe và lớp học: đi giữa aisle, actor đứng phía trước foreground và pose ngồi sau bàn. Chưa thay thế artistic motion QA hoặc thiết bị thực.
- M07 queue/deadline/chuyến sau/montage/ledger/tín chỉ và M08 lẩu→M09 còn pending. Không đánh full game hoàn tất.
- Audio dùng cue tổng hợp hiện có; chưa nghiệm thu mix, touch/device/build, full route hoặc artistic animation QA.

## Bằng chứng và kiểm tra thực tế

- `review/opening-playmode/report.json`:390checks, fullM01–M06cả hai giường. TrongM06: đi bộ vàE qua các anchor; từ chốiWC Nữ; WC Nam/collider/physics; save/load tại balo/WC/H6/404; đợi xe thật; double input không trừ hai vé; puzzle sai/retry/cancel/đúng; về kểĐạm; mởM07không nhảy năm/tín chỉ.
- `review/production-campaign-model.json`:346.807assertions/192tổ hợp/199event, SHA khớp definition hiện tại; thêm kiểm tra prefix compatibility tớiM05và destination mớiM06.
- `review/opening-runtime-evidence.json`:149file source/scene/asset/meta/report/capture cóSHA; collector từ chối report cũ hơn source/scene. `collect_opening_evidence.py` yêu cầu ghi nhận visual review rõ ràng.
- Visual correction không sinh ảnh mới: WC collider offset theo vùng đi bộ, hành lang footy≈-.75, classroom spawn giữa aisle/x bounds±1.6, study posex-.95/y.1/scale.65sau desk foreground.
