import Image from 'next/image';

import { X } from "lucide-react"
import { Button } from "@/components/ui/button"

interface FullScreenImageProps {
    src: string;
    alt: string;
    onClose: () => void;
};

const FullScreenImage = ({ src, alt, onClose }: FullScreenImageProps) => {
    return (
        <div className="fixed inset-0 bg-black bg-opacity-90 flex items-center justify-center z-50">
            <Button
                variant="ghost"
                size="icon"
                className="absolute top-4 right-4 text-white"
                onClick={onClose}
            >
                <X className="h-6 w-6" />
            </Button>
            <Image src={src} alt={alt} className="max-w-full max-h-full object-contain" />
        </div>
    )
}

export default FullScreenImage;