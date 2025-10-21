import 'package:flutter/material.dart';
import 'package:model_viewer_plus/model_viewer_plus.dart';

class ModelViewPage extends StatefulWidget {
  const ModelViewPage({super.key});
  @override
  State<ModelViewPage> createState() => _ModelViewPageState();
}

class _ModelViewPageState extends State<ModelViewPage> {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('Model 3D (GLB)')),
      body: LayoutBuilder(
        builder: (context, c) {
          final half = c.maxHeight * 0.5;
          return Column(
            children: [
              SizedBox(
                height: half,
                width: double.infinity,
                child: const ModelViewer(
                  src: 'assets/models/PartDesignExample-Body.glb',
                  cameraControls: true,
                  autoRotate: true,
                  disableZoom: false,
                  backgroundColor: Color(0xFFFFFFFF),
                ),
              ),
              Expanded(
                child: Container(
                  width: double.infinity,
                  color: Colors.grey.shade100,
                  padding: const EdgeInsets.all(16),
                  child: const Text(
                    'Phần dưới dành cho thông tin / nút điều khiển.\n'
                    '- Dùng gesture mặc định: kéo = xoay, pinch = zoom.',
                    style: TextStyle(fontSize: 15),
                  ),
                ),
              )
            ],
          );
        },
      ),
    );
  }
}