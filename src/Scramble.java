import java.io.IOException;
import java.awt.image.*;
import java.awt.*;
import javax.swing.*;

import java.util.Random;

public class Scramble {
    public static void main(String args[]) throws IOException, AWTException {
        Random r = new Random();
        JFrame frame = new JFrame();
        frame.setAlwaysOnTop(true);
        frame.setExtendedState(JFrame.MAXIMIZED_BOTH);
        frame.setDefaultCloseOperation(WindowConstants.DO_NOTHING_ON_CLOSE);
        frame.setUndecorated(true);
        frame.setType(JFrame.Type.UTILITY);
        frame.setBackground(new Color(0, 0, 0, 0));
        frame.setOpacity(0);
        for (var l = 0; true; l = (l + 1) % 100) {
            frame.setAlwaysOnTop(false);
            frame.setAlwaysOnTop(true);
            BufferedImage img = null, img2 = null;
            // read image
            Robot robot = new Robot();
            Rectangle screenRect = new Rectangle(Toolkit.getDefaultToolkit().getScreenSize());
            img2 = robot.createScreenCapture(screenRect);
            img = new BufferedImage(img2.getWidth(), img2.getHeight(), BufferedImage.TYPE_INT_ARGB);
            // f = new File("Sample.jpg");
            // g = new File("Sample.jpg");
            // img = ImageIO.read(f);
            // img2 = ImageIO.read(g);

            // get image width and height
            int width = img.getWidth();
            int height = img.getHeight();

            /**
             * to keep the project simple we will set the ARGB value to 255, 100, 150 and
             * 200 respectively.
             */
            int mk = r.nextInt(10);
            for (var k = 0; k < mk; k++) {
                int x0 = r.nextInt(width) + 1;
                int y0 = r.nextInt(height) + 1;
                int x = r.nextInt(width) + 1;
                int y = r.nextInt(height) + 1;
                // x = x0;
                // y = y0;
                int mi = r.nextInt(width) + 1;
                int mj = r.nextInt(height) + 1;
                int addr = r.nextInt(255) - 255 / 2;
                int addg = r.nextInt(255) - 255 / 2;
                int addb = r.nextInt(255) - 255 / 2;
                int num = r.nextInt(10);
                if (r.nextBoolean()) {
                    if (r.nextBoolean()) {
                        mi = r.nextInt(width / 10) + 1;
                        mj = r.nextInt(height) + 1;
                    } else {
                        mi = r.nextInt(width) + 1;
                        mj = r.nextInt(width / 10) + 1;
                    }
                }
                img2 = robot.createScreenCapture(screenRect);
                for (int i = 0; i < mi; i++) {
                    for (int j = 0; j < mj; j++) {
                        Color color, color2;
                        switch (num) {
                            case 0 | 1 | 2 | 3:
                                color = new Color(img2.getRGB((x0 + i) % width, (y0 + j) % height));
                                try {
                                    color2 = new Color(addr + 255 / 2, addg + 255 / 2, addb + 255 / 2);
                                } catch (Exception e) {
                                    color2 = color;
                                }
                                img.setRGB((x + i) % width, (y + j) % height, color2.getRGB());
                                break;
                            case 4:
                                color = new Color(img2.getRGB((x0 + i) % width, (y0 + j) % height));
                                try {
                                    color2 = new Color((r.nextInt(255) - 255 / 2) + 255 / 2, (r.nextInt(255) - 255 / 2) + 255 / 2,
                                            (r.nextInt(255) - 255 / 2) + 255 / 2);
                                } catch (Exception e) {
                                    color2 = color;
                                }
                                img.setRGB((x + i) % width, (y + j) % height, color2.getRGB());
                                break;
                            case 5 | 6 | 7 | 8:
                                color = new Color(img2.getRGB((x0 + i) % width, (y0 + j) % height));
                                try {
                                    color2 = new Color(color.getRed() + addr, color.getGreen() + addg,
                                            color.getBlue() + addb);
                                } catch (Exception e) {
                                    color2 = color;
                                }
                                img.setRGB((x + i) % width, (y + j) % height, color2.getRGB());
                                break;
                            case 9:
                                color = new Color(img2.getRGB((x0 + i) % width, (y0 + j) % height));
                                try {
                                    color2 = new Color(color.getRed() + (r.nextInt(255) - 255 / 2),
                                            color.getGreen() + (r.nextInt(255) - 255 / 2),
                                            color.getBlue() + (r.nextInt(255) - 255 / 2));
                                } catch (Exception e) {
                                    color2 = color;
                                }
                                img.setRGB((x + i) % width, (y + j) % height, color2.getRGB());
                                break;

                        }
                    }
                }
                // System.out.println(num);
            }
            ImageIcon icon = new ImageIcon(img);
            JLabel label = new JLabel(icon);
            frame.add(label);
            frame.setOpacity(1);
            frame.repaint();
            if (l == 0) {
                frame.setVisible(true);
            }

            // try {
            //     Thread.sleep(300);
            // } catch (InterruptedException e) {
            //     e.printStackTrace();
            // }

            label.setIcon(null);
            frame.setOpacity(0);
            frame.repaint();
            System.gc();
        }
    }// main() ends here
}// class ends here