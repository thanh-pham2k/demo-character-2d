# Máy tính và tiến độ Năm 1

Luồng: ngồi ghế → Mở máy → Windows → GAME hoặc LỊCH HỌC. GAME có LOL, FC Online, Valorant, CS2; đây là các phiên chơi mô phỏng trong game, không khởi chạy trò chơi bên ngoài. Thoát phiên trở lại Windows; tắt máy hoặc đứng lên hủy phiên và lưu thời gian đã dùng.

Một giây gameplay tiêu 4 phút trong thế giới (15 giây = 1 giờ). Đồng hồ tiến qua sáng, trưa, chiều, tối, dừng ở 24:00; đổi ngày vẫn qua luồng ngủ hiện có, tránh tự động bỏ qua nhiệm vụ năm học. Xem lịch hoặc Windows không tiêu thời gian chơi. Menu học/startup tạm dừng phiên. Thời gian, tổng giây chơi và lịch sử điểm danh nằm trong YearOneProgress, lưu định kỳ 5 giây và khi thoát. Phiên đang chạy không được khôi phục tự động khi Continue.

Hai ca đại diện: 08–10 và 10–12. Đi học sau 15 phút được ghi đi trễ; chơi qua giờ kết thúc ghi vắng. Buổi vắng không cấp kết quả học, không mở bài tập/ôn/thi phụ thuộc. Lịch học hiển thị cả hai học kỳ, tín chỉ hiện có, môn còn nợ do vắng và số môn chưa đạt. Học bù mất 15 giây gameplay mỗi buổi; hủy giữa chừng không được tính buổi. Học bù khôi phục điều kiện học tập thông qua cùng dữ liệu môn học, không cấp trực tiếp tín chỉ.

Khi nghỉ cả sáng ở phòng, điểm danh ghi ở nhà và hoàn tất yêu cầu di chuyển trong ngày mà không trừ tiền vé xe chưa đi. Điều kiện học, ngủ, thi, tổng kết 32 tín chỉ vẫn đi qua YearOneProgress. Luồng Long tối ngày 2 dùng cùng phiên chơi/đồng hồ thay cho bộ đếm riêng.

Mã: DeskActionModal → ComputerDesktopUI → YearOneCampaignController (partial YearOneComputerController) → YearOneProgress (partial YearOneComputerTime). Không tạo hệ thống lịch học, tín chỉ hoặc lưu game thứ hai. Không sinh thêm ảnh.

Kiểm thử: yêu cầu Temp/ComputerFlowChecks.run chạy dữ liệu độc lập và Play Mode dùng save sandbox; báo cáo/ảnh ở PlayModeInteractionReports/computer-flow. YearOneProgressChecks.run kiểm tra hồi quy cả sáu ngày, vé xe, thi, 32 tín chỉ và checkpoint.

Kết quả cuối: ComputerFlowChecks PASS, gồm nhận chuột trên nút game, bốn game, đồng hồ chạy/dừng/tạm dừng trong Play Mode, hủy/hoàn tất học bù bằng thời gian thực, lưu/tải giờ và nợ, đi trễ/vắng và phục hồi đủ sáu ngày tới 32 tín chỉ trong mô hình. Hồi quy YearOneProgressChecks: 117/117 đạt. Ảnh Windows/Game/Lịch học đã được xem lại; cửa sổ có Canvas riêng để không bị đồ vật trong phòng che và chặn click xuyên nền.
