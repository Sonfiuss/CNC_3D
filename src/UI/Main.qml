import QtQuick 2.15
import QtQuick.Controls 2.15

ApplicationWindow {
    visible: true
    width: 600
    height: 600
    title: qsTr("Điều khiển Máy CNC 3 Trục")

    Column {
        anchors.centerIn: parent
        spacing: 18

        // Tiêu đề
        Text {
            text: "Điều khiển Máy CNC 3 Trục"
            font.bold: true
            font.pointSize: 18
            horizontalAlignment: Text.AlignHCenter
            width: parent.width
        }

        // Loại mũi khoan
        Row {
            spacing: 10
            Label { text: "Loại mũi khoan:" }
            ComboBox {
                id: bitType
                model: ["Mũi thẳng", "Mũi V", "Mũi tròn", "Mũi côn"]
            }
        }

        // Loại chất liệu gỗ
        Row {
            spacing: 10
            Label { text: "Chất liệu gỗ:" }
            ComboBox {
                id: woodType
                model: ["Gỗ thông", "Gỗ sồi", "Gỗ xoan đào", "Gỗ MDF"]
            }
        }

        // Nhập kích thước gỗ
        Row {
            spacing: 10
            Label { text: "Kích thước gỗ (mm):" }
            TextField { id: lengthField; placeholderText: "Dài (L)" ; width: 60 }
            TextField { id: widthField; placeholderText: "Rộng (W)" ; width: 60 }
            TextField { id: heightField; placeholderText: "Dày (H)" ; width: 60 }
        }

        // Xác nhận thông số
        Button {
            text: "Xác nhận thông số"
            onClicked: {
                infoLabel.text = "Mũi khoan: " + bitType.currentText
                                + ", Gỗ: " + woodType.currentText
                                + ", Kích thước: " + lengthField.text + " x "
                                + widthField.text + " x " + heightField.text + " mm";
            }
        }

        // Hiển thị thông số đã chọn
        Label {
            id: infoLabel
            text: ""
            font.italic: true
            color: "#2255AA"
        }

        // Khu vực điều khiển máy
        Row {
            spacing: 15
            Button { text: "Bắt đầu gia công"; onClicked: statusLabel.text = "Đang gia công..." }
            Button { text: "Tạm dừng"; onClicked: statusLabel.text = "Đã tạm dừng!" }
            Button { text: "Dừng khẩn cấp"; onClicked: statusLabel.text = "Dừng khẩn cấp!" }
        }
        Label {
            id: statusLabel
            text: "Trạng thái máy: Chờ lệnh"
            font.bold: true
            color: "#BB2222"
        }

        // Nhật ký quá trình
        Rectangle {
            width: parent.width * 0.9
            height: 120
            color: "#f6f6f6"
            border.color: "#cccccc"
            radius: 6

            TextArea {
                anchors.fill: parent
                placeholderText: "Nhật ký quá trình sẽ hiển thị ở đây..."
                readOnly: true
            }
        }
    }
}
